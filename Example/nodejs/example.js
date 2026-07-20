// ============================================================
// Node.js Example - DWG to SVG Conversion
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//   npm install
//
// Run: node example.js
// ============================================================

try {
    const ole = require("win32-ole");
    const objImage = new ole.ActiveXObject("DWG2SVGX.DWG2SVG");

    // --- Basic Settings ---

    objImage.Invoke("AddFontPath", "C:\\fonts\\");
    objImage.Put("ColorType", 0);       // 0=True Color, 1=B&W, 2=Gray
    objImage.Put("LayoutType", 0);      // 0=All, 1=Model, 2=Paper
    objImage.Invoke("SetSize", 800, 600);

    // --- Advanced Settings (optional) ---

    // objImage.Put("OutputLayer", 0);
    // objImage.Put("ConvertAsLayoutSizeOut", 0);
    // objImage.Put("HatchType", 0);
    // objImage.Put("Outputtype", 0);
    // objImage.Put("ExplodeShxText", 0);
    // objImage.Put("LineWeightScale", 1.0);
    // objImage.Put("OutScale", 1.0);
    // objImage.Invoke("SetConvertType", "model");

    // --- Convert ---
    objImage.Invoke("Convert", "f:\\2018.dwg", "f:\\");

    console.log("Conversion completed.");
} catch (e) {
    console.log("Conversion failed. Please check the input file and settings.");
    console.log("Error:", e.message);
}