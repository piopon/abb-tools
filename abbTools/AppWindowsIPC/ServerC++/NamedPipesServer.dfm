object namedPipeServer: TnamedPipeServer
  Left = 0
  Top = 0
  BorderStyle = bsToolWindow
  Caption = 'test pipe server'
  ClientHeight = 301
  ClientWidth = 227
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -13
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  PixelsPerInch = 120
  TextHeight = 16
  object chkOpenCloseServer: TCheckBox
    Left = 13
    Top = 13
    Width = 202
    Height = 25
    Caption = 'OPEN PIPE SERVER'
    TabOrder = 0
    OnClick = chkOpenCloseServerClick
  end
  object statusBar: TStatusBar
    Left = 0
    Top = 282
    Width = 227
    Height = 19
    Panels = <
      item
        Text = 'app running...'
        Width = 50
      end>
  end
  object btnSendStart: TButton
    Left = 13
    Top = 48
    Width = 202
    Height = 52
    Caption = 'send START'
    TabOrder = 2
    OnClick = btnSendStartClick
  end
  object btnSendStop: TButton
    Left = 13
    Top = 110
    Width = 202
    Height = 52
    Caption = 'send STOP'
    TabOrder = 3
    OnClick = btnSendStopClick
  end
  object grpUserMsg: TGroupBox
    Left = 13
    Top = 170
    Width = 202
    Height = 100
    Caption = ' user message '
    TabOrder = 4
    object edtMsg: TEdit
      Left = 25
      Top = 26
      Width = 152
      Height = 24
      Alignment = taCenter
      TabOrder = 0
      Text = 'message'
    end
    object btnSendCurr: TButton
      Left = 25
      Top = 56
      Width = 152
      Height = 30
      Caption = 'send'
      TabOrder = 1
      OnClick = btnSendCurrClick
    end
  end
end
