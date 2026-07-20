# ============================================================
# PowerShell Example - DWG to SVG Conversion
#
# Prerequisites:
#   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
#   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
#
# Run: powershell -ExecutionPolicy Bypass -File example.ps1
# ============================================================

try {
    $objImage = New-Object -ComObject DWG2SVGX.DWG2SVG

    # --- Basic Settings ---

    # Add font search path
    $objImage.AddFontPath("C:\fonts\")

    # Color mode: 0 = True Color, 1 = Black & White, 2 = Gray
    $objImage.ColorType = 0

    # Layout: 0 = All Layouts, 1 = Model Space, 2 = Paper Space
    $objImage.LayoutType = 0

    # Output size in pixels
    $objImage.SetSize(800, 600)

    # --- Advanced Settings (optional) ---

    # $objImage.OutputLayer = 0
    # $objImage.ConvertAsLayoutSizeOut = 0
    # $objImage.HatchType = 0
    # $objImage.Outputtype = 0
    # $objImage.ExplodeShxText = 0
    # $objImage.LineWeightScale = 1.0
    # $objImage.OutScale = 1.0
    # $objImage.SetConvertType("model")

    # --- Convert ---
    $objImage.Convert("f:\2018.dwg", "f:\")

    Write-Host "Conversion completed."

    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($objImage) | Out-Null
}
catch {
    Write-Host "Conversion failed. Please check the input file and settings."
    Write-Host ("Error code: 0x{0:X8}" -f ($_.Exception.HResult -band 0xFFFFFFFF))
}