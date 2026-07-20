# DWG2SVGX Multi-Language Examples

This directory contains examples in multiple programming languages demonstrating how to call the DWG to SVG conversion functionality through the COM interface.

## Prerequisites

All examples require the COM DLL to be registered (run as Administrator):

```cmd
regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
```

## Examples Overview

### Command-Line Examples

Simple command-line programs demonstrating the basic conversion workflow:

| Directory | Language | How to Run | Description |
|-----------|----------|-----------|-------------|
| `vbscript` | VBScript | `cscript example.vbs` | The simplest approach, no compilation needed |
| `csharp` | C# | `csc /out:Example.exe Example.cs` then run `Example.exe` | Uses `Type.GetTypeFromProgID` + `dynamic` to call COM |
| `python` | Python | `python example.py` | Requires `pip install pywin32`, uses `win32com.client.Dispatch` |
| `powershell` | PowerShell | `powershell -ExecutionPolicy Bypass -File example.ps1` | Uses `New-Object -ComObject` |
| `java` | Java | See README in directory | Uses Jacob library's `ActiveXComponent` |
| `php` | PHP | `php example.php` | Requires `extension=com_dotnet` in php.ini |
| `nodejs` | Node.js | `npm install` then `node example.js` | Uses `win32-ole` library |
| `go` | Go | `go mod tidy` then `go run example.go` | Uses `go-ole/oleutil` library |

### Desktop GUI Examples

Graphical applications exposing all available parameters:

| Directory | Tech Stack | How to Run | Description |
|-----------|-----------|-----------|-------------|
| `csharp-winform` | C# WinForms (.NET 8) | `dotnet run` | Windows native desktop app with basic and advanced settings panels |
| `python-gui` | Python tkinter | `python example_gui.py` | Cross-platform desktop app with the same features as the WinForms version |

GUI features:
- **Basic Settings**: ColorType, LayoutType, output size (W x H), font path
- **Advanced Settings**: OutputLayer, HatchType, OutputType, LineWeightScale, OutScale, ConvertType, ConvertAsLayoutSizeOut, ExplodeShxText

### Web Examples

Browser-based conversion tools with file upload and download:

| Directory | Tech Stack | How to Run | URL |
|-----------|-----------|-----------|-----|
| `aspnet` | ASP.NET Core (.NET 8) + Razor Pages | `dotnet run` | `http://localhost:9878` |
| `php-web` | PHP built-in server | `php -S localhost:9879` | `http://localhost:9879` |

Web features:
- Upload DWG files
- Collapsible settings panel (auto-collapses after conversion)
- Basic + Advanced settings (consistent with GUI versions)
- Display generated SVG file list after conversion
- Download links for each SVG file
- Popup notification on completion

## API Reference

All examples call the same COM interface with ProgID `DWG2SVGX.DWG2SVG`.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `ColorType` | short | Color mode: 0=True Color, 1=B&W, 2=Gray |
| `LayoutType` | short | Layout type: 0=All Layouts, 1=Model Space, 2=Paper Space |
| `OutputLayer` | short | Output layer control |
| `ConvertAsLayoutSizeOut` | short | Output by layout size (0=No, 1=Yes) |
| `HatchType` | short | Hatch pattern handling mode |
| `Outputtype` | short | Output type setting |
| `ExplodeShxText` | short | Explode SHX text (0=No, 1=Yes) |
| `LineWeightScale` | double | Line weight scale factor |
| `OutScale` | float | Output scale factor |
| `Appearance` | short | Appearance setting |

### Methods

| Method | Parameters | Description |
|--------|-----------|-------------|
| `Convert` | `(string InputDWGFile, string OutputFolder)` | Execute conversion |
| `SetSize` | `(int Width, int Height)` | Set output SVG viewport size in pixels |
| `AddFontPath` | `(string Path)` | Add font search path |
| `SetConvertType` | `(string Model)` | Set conversion type |

### Output File Naming

Generated SVG files follow the naming pattern: `{DWG filename}-{layout name}.svg`

For example, converting `2018.dwg` may produce:
- `2018-Model.svg`
- `2018-Layout1.svg`
- `2018-Layout2.svg`

## Notes

1. **Platform**: COM components only work on Windows
2. **Bit matching**: 64-bit programs require 64-bit DLL registration; 32-bit programs require 32-bit DLL
3. **Font path**: If the DWG uses custom fonts, use `AddFontPath` to specify the font directory
4. **Error handling**: All examples include error handling that displays error codes (not system-localized messages) on failure