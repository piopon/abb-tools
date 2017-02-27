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
		AnsiString msgSend;
		AnsiString msgRecv;
		//message sending and receiving
		bool send(void);
		bool receive(void);
		//client service
		void waitForClient(void);
		bool checkConn(void);
		//background communication (separate thread = static)
		static DWORD WINAPI backgroundComm(LPVOID lpvParam);
	public:
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//constructor, copy constructor and destructor
		PipeServer(AnsiString name);
		PipeServer(const PipeServer &org);
		~PipeServer();
		//open and close pipe server
		bool open(void);
		void close(void);
		//
		void send(AnsiString message);
		//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//on client connected
		void (*onClientConnected)(void);
		void (*onClientDisconnected)(void);
		//on messaging
		void (*onSent)(AnsiString sentMsg);
		void (*onReceive)(AnsiString recvMsg);
		//on communication thread end
		void (*onCommEnd)(void);
	protected:
};

#endif
