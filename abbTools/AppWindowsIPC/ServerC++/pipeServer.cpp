//---------------------------------------------------------------------------
#pragma hdrstop
#include "pipeServer.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
//------------------------------------------------------------------------------
PipeServer::PipeServer(AnsiString name, bool autoRecon)
{
	myPipe = INVALID_HANDLE_VALUE;
	myName = L"\\\\.\\pipe\\" + name;
	myBuffer = 512;
	myConn = PIPE_ACCESS_DUPLEX | FILE_FLAG_OVERLAPPED;
	myMode = PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT;
	myInstances = PIPE_UNLIMITED_INSTANCES;
	//no client + clear message queue
	clientConn = false;
	userClose = false;
	restoreConn = autoRecon;
	msgSend = "";
	msgRecv = "";
	//clear all events
	onCommEnd = NULL;
	onClientConnected = NULL;
	onClientDisconnected = NULL;
	onClientWaiting = NULL;
	onSent = NULL;
    onReceive = NULL;
}
//------------------------------------------------------------------------------
PipeServer::PipeServer(const PipeServer &org)
{
	myPipe = org.myPipe;
	myName = org.myName;
	myBuffer = org.myBuffer;
	myConn = org.myConn;
	myMode = org.myMode;
	myInstances = org.myInstances;
	//copy client status + message queue
	clientConn = org.clientConn;
	userClose = org.userClose;
	restoreConn = org.restoreConn;
	msgSend = org.msgSend;
	msgRecv = org.msgRecv;
	//copy all events
	onCommEnd = org.onCommEnd;
	onClientConnected = org.onClientConnected;
	onSent = org.onSent;
	onReceive = org.onReceive;
}
//------------------------------------------------------------------------------
PipeServer::~PipeServer()
{
	myPipe = INVALID_HANDLE_VALUE;
	myName = "";
	myBuffer = 0;
	myConn = 0;
	myMode = 0;
	myInstances = 0;
	//no client + clear message queue
	clientConn = false;
	userClose = false;
	restoreConn = false;
	msgSend = "";
	msgRecv = "";
	//clear all events
	onCommEnd = NULL;
	onClientConnected = NULL;
	onClientDisconnected = NULL;
	onClientWaiting = NULL;
	onSent = NULL;
    onReceive = NULL;
}
//------------------------------------------------------------------------------
bool PipeServer::open(void)
{
	//get security access
	SECURITY_ATTRIBUTES mySecure = setSecurity();
	//create named pipe and get handle to it
	myPipe = CreateNamedPipe(myName.c_str(),myConn,myMode,myInstances,myBuffer,myBuffer,0,&mySecure);
	if(myPipe != INVALID_HANDLE_VALUE) {
		//set initial data
		clientConn = false;
		userClose = false;
		//clear all buffer
		msgSend = "";
		msgRecv = "";
		//wait for client connection in a background thread
		HANDLE hThread = CreateThread(NULL,0,backgroundComm,this,0,NULL);
		if (hThread == NULL) {
			ShowMessage("CreateThread failed "+FloatToStr(GetLastError()));
			return false;
		} else {
			//we dont need handle to thread
			CloseHandle(hThread);
			//all is OK - communication running
			return true;
		}
	} else {
        return false;
    }
}
//------------------------------------------------------------------------------
bool PipeServer::reopen(void)
{
	//get security access
	SECURITY_ATTRIBUTES mySecure = setSecurity();
	//create named pipe and get handle to it
	myPipe = CreateNamedPipe(myName.c_str(),myConn,myMode,myInstances,myBuffer,myBuffer,0,&mySecure);
	if(myPipe != INVALID_HANDLE_VALUE) {
		//set initial data
		clientConn = false;
		userClose = false;
		//clear all buffer
		msgSend = "";
		msgRecv = "";
		//all is OK
		return true;
	} else {
        return false;
    }
}
//------------------------------------------------------------------------------
void PipeServer::close(void)
{
	//flush data and destroy (close) handle
	if(myPipe != INVALID_HANDLE_VALUE){
		if(clientConn){
			FlushFileBuffers(myPipe);
			DisconnectNamedPipe(myPipe);
		}
		//if not connected
		if (!CloseHandle(myPipe)){
			ShowMessage("CloseHandle failed "+FloatToStr(GetLastError()));
		} else {
			//to finish background thread
			myPipe = INVALID_HANDLE_VALUE;
        }
	}
}
//------------------------------------------------------------------------------
bool PipeServer::send(void)
{
	bool result = clientConn;
	if(clientConn && msgSend!=""){
		DWORD numBytesWritten = 0;
		msgSend += '\0';
		result = WriteFile(myPipe,msgSend.c_str(),msgSend.Length(),&numBytesWritten,NULL);
		//trigger client sent event
		if (onSent!=NULL) onSent(msgSend);
		//clear queue
		msgSend = "";
	}
	//exit from function
	return result;
}
//------------------------------------------------------------------------------
void PipeServer::send(AnsiString message)
{
	msgSend = message;
}
//------------------------------------------------------------------------------
bool PipeServer::receive(void)
{
	bool result = false;
	if(clientConn){
        unsigned long bytesToRead = 0, bytesReaded = 0;
		//peek for new message
		if(PeekNamedPipe(myPipe,NULL,NULL,NULL,&bytesToRead,NULL)) {
			if (bytesToRead>0) {
				//we have new message -
				char buffer[128];
				if (ReadFile(myPipe,buffer,bytesToRead,&bytesReaded,NULL)){
					if (bytesReaded>0) {
						//get only interesting part of buffer
						msgRecv = buffer;
						msgRecv = msgRecv.SubString(0,bytesReaded);
						//trigger client receive event
						if (onReceive!=NULL) onReceive(msgRecv);
						//result ok
						result = true;
					}
				} else {
					result = false;
				}
			}
        }
	}
	return result;
}
//------------------------------------------------------------------------------
SECURITY_ATTRIBUTES PipeServer::setSecurity()
{
	SECURITY_ATTRIBUTES result;

	result.lpSecurityDescriptor = (PSECURITY_DESCRIPTOR)malloc(SECURITY_DESCRIPTOR_MIN_LENGTH);
	InitializeSecurityDescriptor(result.lpSecurityDescriptor, SECURITY_DESCRIPTOR_REVISION);
	// ACL is set as NULL in order to allow all access to the object.
	SetSecurityDescriptorDacl(result.lpSecurityDescriptor, TRUE, NULL, FALSE);
	result.nLength = sizeof(result);
	result.bInheritHandle = true;

	return result;
}
//------------------------------------------------------------------------------
void PipeServer::waitForClient(void)
{
	while (!clientConn){
		if(myPipe == INVALID_HANDLE_VALUE){
			break;
		}
		if (ConnectNamedPipe(myPipe, NULL)) {
			clientConn = true;
			//clear queue
			msgSend = "";
			msgRecv = "";
		} else {
			clientConn = GetLastError() == ERROR_PIPE_CONNECTED;
			//we cant get here only if user closed server from GUI
			userClose = !clientConn;
		}
	}
}
//------------------------------------------------------------------------------
bool PipeServer::checkConn(void)
{
	bool result = false;
	//pipe must be created to check it status
	if(myPipe != INVALID_HANDLE_VALUE) {
		//check status of pipe
		if(!PeekNamedPipe(myPipe, NULL, NULL, NULL, NULL, NULL) ){
            //pipe NOK - check why
			DWORD dwError = GetLastError();
			if((dwError == ERROR_BROKEN_PIPE)||(dwError == ERROR_PIPE_NOT_CONNECTED)){
				//trigger client disconnected event
				if (onClientDisconnected!=NULL) onClientDisconnected();
				userClose = false;
			} else if (dwError == ERROR_INVALID_HANDLE){
				//do nothing here (break outside)
				userClose = true;
			}
		} else {
			//pipe OK
			result = true;
        }
	} else {
		//closed by GUI - terminate anyways
		userClose = true;
	}
	//update client connected flag
	clientConn = result;
	//exit method
	return result;
}
//------------------------------------------------------------------------------
bool PipeServer::checkRestoreComm(bool wasConnected)
{
	bool result = false;
	if(wasConnected) {
		if(restoreConn && !userClose) {
            //restore connection = close server
			close();
			//re-open new connection
			result = reopen();
		} else {
			result = false;
		}
	} else {
		//not connected yet - restore only if not closed by user
		result = !userClose;
	}
	return result;
}
//------------------------------------------------------------------------------
DWORD WINAPI PipeServer::backgroundComm(LPVOID lpvParam)
{
	PipeServer *currPipe = (PipeServer*)lpvParam;
	// check for errors
	if (lpvParam == NULL) {
		ShowMessage("ERROR::backgroundWait - lpvParam = NULL");
		return -1;
	}
	bool connOK = false;
	//check if we want to auto reconect
	while (currPipe->checkRestoreComm(connOK)) {
		//trigger client waiting event
		if (currPipe->onClientWaiting!=NULL) currPipe->onClientWaiting();
		//wait for client in background
		currPipe->waitForClient();
		//run user function
		if(currPipe->clientConn && currPipe->onClientConnected != NULL) {
			currPipe->onClientConnected();
			connOK = true;
		}
		//trigger client connected event
		while (currPipe->clientConn) {
			//do loop while handle exists (gui thread closes server or clien disconn)
			if (!currPipe->checkConn()) break;
			//check if there is something to receive
			currPipe->receive();
			//check if there is something to send
			currPipe->send();
			//sleep thread to aviod heavy processor load
			Sleep(100);
		}
		//client disconnected flag and trigger in checkConn action
	}
	//execution action at communication thread end
	if (currPipe->onCommEnd!=NULL) currPipe->onCommEnd();
	return 1;
}
//------------------------------------------------------------------------------
