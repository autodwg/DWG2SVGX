# PHP Web - DWG to SVG Converter (Full API)

A PHP web-based DWG to SVG converter with file upload and full API settings.

## Prerequisites

Register the COM DLL (run as Administrator):
```cmd
regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
```

Enable COM extension in `php.ini`:
```ini
extension=com_dotnet
```

## Run
```bash
php -S localhost:9879
```

Open browser to `http://localhost:9879`

## Web Features

- File upload for DWG files
- Basic settings: ColorType, LayoutType, Size (W/H), FontPath
- Advanced settings: OutputLayer, HatchType, OutputType, LineWeightScale, OutScale, ConvertType, ConvertAsLayoutSizeOut, ExplodeShxText
- Download links for generated SVG files
- Popup notification on completion

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