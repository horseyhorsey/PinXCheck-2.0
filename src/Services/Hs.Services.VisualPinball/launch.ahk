return

LoadVP(WPath, Exe)
{	
	IfWinExist ,ahk_class VPinball
		WinActivate, ahk_class VPinball	
	else{
		Run, %WPath%\%Exe%
		WinWait, ahk_class VPinball
		WinActivate, ahk_class VPinball	
		}
}

LoadTableToEditor(TablePath, TableName)
{
	Send, {Control Down}{o}{Control Up}
	WinWait, Open ahk_class #32770
	Sleep 200
	WinActivate, Open ahk_class #32770
	ControlClick, ToolbarWindow323, Open ahk_class #32770 ,,,, NA x192 x0
	Clipboard = % TablePath
	Send, ^v
	Send {Enter}
	ControlClick, Edit1, Open ahk_class #32770
	Clipboard = % TableName
	Send, ^v{50}{Enter}
}

PlayTable(vp, table, vpx)
{
	if vpx
		Run, %vp% /Play -"%table%".vpx
	else
		Run, %vp% /Play -"%table%".vpt

	;VPReady :=false
	;while VPReady=0
	;{
	;	GoSub Disclaimer
	;	IfWinExist, Microsoft Visual ahk_class #32770
	;	{
	;		WinActivate, Microsoft Visual ahk_class #32770
	;		GoSub HandleError
	;		break
	;	}
	
	;	IfWinExist, Visual Pinball Player
	;		break
	;}

	;B2S
	;IfWinExist, Form1
	;{
	;	WinActivate, Form1
	;	WinActivate, Visual Pinball Player
	;	}

	;WinWaitClose, Visual Pinball Player
	;WinClose, ahk_class VPinball
}		

ActivateScript(vpx)
{
	if vpx
	{
		Send {Alt Down}{v}{Alt Up}{s}
	}
	else
	{
		Send {Alt Down}{e}{Alt Up}{s}
		ControlClick,Scintilla1, ahk_class CVFrame	
	}
}

Disclaimer:
IfWinExist, Please ahk_class #32770
{
	WinActivate, Please ahk_class #32770
	Send {Tab Down}{Tab Up}
	Send {Space Down}{Space Up}
	Send {Tab Down}{Tab Up}
	Send {Enter Down}{Enter Up}
    return
}
return

HandleError:
IfWinExist, Microsoft Visual ahk_class #32770
{
	WinActivate, Microsoft Visual ahk_class #32770
	Send {Enter Down}{Enter Up}
    return
}
return