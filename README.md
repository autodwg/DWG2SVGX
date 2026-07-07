# DWG2SVGX
**AutoDWG DWG to SVG Component**

## License Notice
1. Free trial / non-commercial use: GNU LGPLv3
2. Commercial production use, closed-source integration requires purchasing our commercial license.

Contact info@autodwg.com for commercial authorization.

## AutoDWG DWG to SVG Control Component
DWG2SVGX is a Control Component let you convert DWG/DXF/DWF file into SVG directly, without need of AutoCAD.

### Key features
- Convert DWG, DXF and DWF file to SVG
- Full Support for Model & Paper Space
- Customizable Font & XREF Paths
- Adjustable SVG Size or Use Layout Page Size
- Color Output Support
- Layer Preservation in Output
- Configurable Hatch Output
- Flexible Handling of SHX Fonts (new function)
- Adjustable Line Weight Scaling

### Free Trial Download Link
https://github.com/autodwg/DWG2SVGX/releases/download/v1.0.0/DWG2SVGX-v1.0.0-Trial.zip 

## User Guide
### Getting Started
Quick setup (The steps below are for 64-bit installation.)

#### Step 1: Register the DLL Component
Double-click `reg.bat` to automatically register `DWG2SVGX64.dll` on your system.

If registration fails:
Open Command Prompt as Administrator via:
Start Menu → Windows System → Right-click "Command Prompt" → Run as Administrator

Manually register the DLL using command:
```cmd
regsvr32 DWG2SVGX64.dll
```
#### Step 2: Test with Example VBScript
Use the provided sample script example.vbs in the Examples folder to verify functionality.
Ensure the script executes without errors and generates the expected SVG output.

**Sample Code**

Sample Code (VB) for your reference:
```
Dim objImage
Set objImage = CreateObject("DWG2SVGX.DWG2SVG")
objImage.AddFontPath "E:\\font\\"
objImage.ColorType = 0 ' 0 true color 1 white/black 2 gray
objImage.LayoutType = 0 ' 0 all layout 1 model space 2 paper space
objImage.SetSize 800,600
objImage.Convert "E:\\test.dwg", "E:\\output\\"
```
