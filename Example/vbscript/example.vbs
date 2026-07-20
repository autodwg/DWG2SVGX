' ============================================================
' DWG to SVG Conversion Example (VBScript)
'
' Prerequisites:
'   Register the COM DLL (run as Administrator):
'     regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
'     regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
'
' Usage:
'   cscript example.vbs
' ============================================================

Option Explicit

Dim objImage
On Error Resume Next

Set objImage = CreateObject("DWG2SVGX.DWG2SVG")
If Err.Number <> 0 Then
    WScript.Echo "Failed to create COM object. Please make sure DWG2SVGX is registered."
    WScript.Quit 1
End If

' --- Basic Settings ---

' Add font search path
objImage.AddFontPath "C:\fonts\"

' Color mode: 0 = True Color, 1 = Black & White, 2 = Gray
objImage.ColorType = 0

' Layout: 0 = All Layouts, 1 = Model Space, 2 = Paper Space
objImage.LayoutType = 0

' Output image size (width, height) in pixels
objImage.SetSize 800, 600

' --- Advanced Settings (optional) ---

' Output layer control
' objImage.OutputLayer = 0

' Output by layout size (0=no, 1=yes)
' objImage.ConvertAsLayoutSizeOut = 0

' Hatch type handling
' objImage.HatchType = 0

' Output type
' objImage.Outputtype = 0

' Explode SHX text (0=no, 1=yes)
' objImage.ExplodeShxText = 0

' Line weight scale factor
' objImage.LineWeightScale = 1.0

' Output scale factor
' objImage.OutScale = 1.0

' Set conversion type
' objImage.SetConvertType "model"

' --- Convert ---
' Convert(inputDWGFile, outputFolder)
objImage.Convert "f:\2018.dwg", "f:\"

If Err.Number <> 0 Then
    WScript.Echo "Conversion failed. Please check the input file and settings."
    WScript.Echo "Error code: 0x" & Hex(Err.Number)
    Set objImage = Nothing
    WScript.Quit 1
End If

WScript.Echo "Conversion completed."

Set objImage = Nothing