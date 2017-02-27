//---------------------------------------------------------------------------

#ifndef NamedPipesServerH
#define NamedPipesServerH
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include "pipeServer.h"
#include <Vcl.ComCtrls.hpp>
//---------------------------------------------------------------------------
class TnamedPipeServer : public TForm
{
__published:	// IDE-managed Components
	TCheckBox *chkOpenCloseServer;
	TStatusBar *statusBar;
	TButton *btnSendStart;
	TButton *btnSendStop;
	TButton *btnSendCurr;
	TEdit *edtMsg;
	TGroupBox *grpUserMsg;
	void __fastcall FormClose(TObject *Sender, TCloseAction &Action);
	void __fastcall chkOpenCloseServerClick(TObject *Sender);
	void __fastcall btnSendStartClick(TObject *Sender);
	void __fastcall btnSendStopClick(TObject *Sender);
	void __fastcall btnSendCurrClick(TObject *Sender);
private:	// User declarations
	PipeServer *myServer;
	static void triggerClientConn(void);
	static void triggerMsgSent(AnsiString sentMsg);
	static void triggerMsgRecv(AnsiString recvMsg);
	static void triggerCommEnd(void);
public:		// User declarations
	__fastcall TnamedPipeServer(TComponent* Owner);

};

//---------------------------------------------------------------------------
extern PACKAGE TnamedPipeServer *namedPipeServer;
//---------------------------------------------------------------------------
#endif
