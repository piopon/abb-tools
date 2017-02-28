//---------------------------------------------------------------------------
#ifndef pipeServerH
#define pipeServerH

#include <windows.h>
#include <vcl.h>
//---------------------------------------------------------------------------
class PipeServer
{
	private:
		//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//named pipe operations data
		HANDLE myPipe;
		String myName;
		unsigned long myBuffer;
		unsigned long myConn;
		unsigned long myMode;
		unsigned long myInstances;
		//named pipe security attributes
		SECURITY_ATTRIBUTES setSecurity(void);
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//server operations data
		bool clientConn;
		bool userClose;
		bool restoreConn;
		AnsiString msgSend;
		AnsiString msgRecv;
		//auto reopen communication (if restoreConn = true)
		bool reopen(void);
		//message sending and receiving
		bool send(void);
		bool receive(void);
		//client service
		void waitForClient(void);
		bool checkConn(void);
		bool checkRestoreComm(bool wasConnected);
		//background communication (separate thread = static)
		static DWORD WINAPI backgroundComm(LPVOID lpvParam);
	public:
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//constructor, copy constructor and destructor
		PipeServer(AnsiString name, bool autoRecon);
		PipeServer(const PipeServer &org);
		~PipeServer();
		//open and close pipe server
		bool open(void);
		void close(void);
		//
		void send(AnsiString message);
		//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//client event handlers
		void (*onClientWaiting)(void);
		void (*onClientConnected)(void);
		void (*onClientDisconnected)(void);
		//messaging event handlers
		void (*onSent)(AnsiString sentMsg);
		void (*onReceive)(AnsiString recvMsg);
		//communication end event handler
		void (*onCommEnd)(void);
	protected:
};

#endif
