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