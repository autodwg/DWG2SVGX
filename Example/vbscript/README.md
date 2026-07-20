# VBScript Example - DWG to SVG Conversion

## Prerequisites

Register the COM DLL (run as Administrator):
```cmd
regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
```

## Run
```cmd
cscript example.vbs
```

## Complete API Reference

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `ColorType` | short | 0=True Color, 1=B&W, 2=Gray |
| `LayoutType` | short | 0=All Layouts, 1=Model Space, 2=Paper Space |
| `OutputLayer` | short | Output layer control |
| `ConvertAsLayoutSizeOut` | short | Output by layout size (0=no, 1=yes) |
| `HatchType` | short | Hatch pattern handling mode |
| `Outputtype` | short | Output type setting |
| `ExplodeShxText` | short | Explode SHX text (0=no, 1=yes) |
| `LineWeightScale` | double | Line weight scale factor |
| `OutScale` | float | Output scale factor |
| `Appearance` | short | Appearance setting |

### Methods

| Method | Parameters | Description |
|--------|-----------|-------------|
| `Convert` | `(string InputDWGFile, string OutputFolder)` | Execute conversion |
| `SetSize` | `(int Width, int Height)` | Set output size in pixels |
| `AddFontPath` | `(string Path)` | Add font search path |
| `SetConvertType` | `(string Model)` | Set conversion type |