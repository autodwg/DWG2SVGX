<?php
// ============================================================
// PHP Example - DWG to SVG Conversion
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//   PHP with COM extension: extension=com_dotnet in php.ini
//
// Run: php example.php
// ============================================================

try {
    $objImage = new COM("DWG2SVGX.DWG2SVG");

    // --- Basic Settings ---

    $objImage->AddFontPath("C:\\fonts\\");
    $objImage->ColorType = 0;       // 0=True Color, 1=B&W, 2=Gray
    $objImage->LayoutType = 0;      // 0=All, 1=Model, 2=Paper
    $objImage->SetSize(800, 600);

    // --- Advanced Settings (optional) ---

    // $objImage->OutputLayer = 0;
    // $objImage->ConvertAsLayoutSizeOut = 0;
    // $objImage->HatchType = 0;
    // $objImage->Outputtype = 0;
    // $objImage->ExplodeShxText = 0;
    // $objImage->LineWeightScale = 1.0;
    // $objImage->OutScale = 1.0;
    // $objImage->SetConvertType("model");

    // --- Convert ---
    $objImage->Convert("f:\\2018.dwg", "f:\\");

    echo "Conversion completed.\n";
} catch (Exception $e) {
    echo "Conversion failed. Please check the input file and settings.\n";
    echo "Error: " . $e->getMessage() . "\n";
}