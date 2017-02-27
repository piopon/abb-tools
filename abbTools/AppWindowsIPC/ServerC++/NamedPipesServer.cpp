//---------------------------------------------------------------------------
#include <vcl.h>
#pragma hdrstop
#include "NamedPipesServer.h"

//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TnamedPipeServer *namedPipeServer;

//---------------------------------------------------------------------------
__fastcall TnamedPipeServer::TnamedPipeServer(TComponent* Owner)
	: TForm(Owner)
{
	myServer = new PipeServer("abc");
	myServer->onClientConnected = &triggerClientConn;
	myServer->onSent = &triggerMsgSent;
	myServer->onReceive = &triggerMsgRecv;
	myServer->onCommEnd = &triggerCommEnd;
}
//---------------------------------------------------------------------------
void __fastcall TnamedPipeServer::FormClose(TObject *Sender, TCloseAction &Action)
{
	delete myServer;
}
//---------------------------------------------------------------------------
void __fastcall TnamedPipeServer::chkOpenCloseServerClick(TObject *Sender)
{
	if (chkOpenCloseServer->Checked) {
		myServer->open();
		statusBar->Panels->Items[0]->Text = "server opened! wait for client...";
		chkOpenCloseServer->Caption = "CLOSE PIPE SERVER";
	} else {
		myServer->close();
		statusBar->Panels->Items[0]->Text = "server closed!";
		chkOpenCloseServer->Caption = "OPEN PIPE SERVER";
    }
}
//---------------------------------------------------------------------------
void __fastcall TnamedPipeServer::btnSendStartClick(TObject *Sender)
{
	myServer->send(L"START");
}
//---------------------------------------------------------------------------

void __fastcall TnamedPipeServer::btnSendStopClick(TObject *Sender)
{
	myServer->send(L"STOP");
}
//---------------------------------------------------------------------------
void __fastcall TnamedPipeServer::btnSendCurrClick(TObject *Sender)
{
	myServer->send(edtMsg->Text);
}
//---------------------------------------------------------------------------
void TnamedPipeServer::triggerClientConn(void)
{
	namedPipeServer->statusBar->Panels->Items[0]->Text = "client connected";
}
//---------------------------------------------------------------------------
void TnamedPipeServer::triggerMsgSent(AnsiString sentMsg)
{
	namedPipeServer->statusBar->Panels->Items[0]->Text = "sent: "+sentMsg;
}
//---------------------------------------------------------------------------
void TnamedPipeServer::triggerMsgRecv(AnsiString recvMsg)
{
	namedPipeServer->statusBar->Panels->Items[0]->Text = "recv: "+recvMsg;
}
//---------------------------------------------------------------------------
void TnamedPipeServer::triggerCommEnd(void)
{
	if (namedPipeServer->chkOpenCloseServer->Checked) {
		//line below will trigger checkbox onClick event
		namedPipeServer->chkOpenCloseServer->Checked = false;
	}
	namedPipeServer->statusBar->Panels->Items[0]->Text = "server closed! thread end!";
}
//---------------------------------------------------------------------------

