// ============================================================
// C# Example - DWG to SVG Conversion
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//
// Build: csc /out:Example.exe Example.cs
// Run:   Example.exe
// ============================================================

using System;
using System.Runtime.InteropServices;

class Example
{
    static void Main()
    {
        try
        {
            Type comType = Type.GetTypeFromProgID("DWG2SVGX.DWG2SVG");
            dynamic objImage = Activator.CreateInstance(comType);

            // --- Basic Settings ---

            // Add font search path
            objImage.AddFontPath(@"C:\fonts\");

            // Color mode: 0 = True Color, 1 = Black & White, 2 = Gray
            objImage.ColorType = 0;

            // Layout: 0 = All Layouts, 1 = Model Space, 2 = Paper Space
            objImage.LayoutType = 0;

            // Output size in pixels
            objImage.SetSize(800, 600);

            // --- Advanced Settings (optional) ---

            // objImage.OutputLayer = 0;
            // objImage.ConvertAsLayoutSizeOut = 0;
            // objImage.HatchType = 0;
            // objImage.Outputtype = 0;
            // objImage.ExplodeShxText = 0;
            // objImage.LineWeightScale = 1.0;
            // objImage.OutScale = 1.0f;
            // objImage.SetConvertType("model");

            // --- Convert ---
            objImage.Convert(@"f:\2018.dwg", @"f:\");

            Console.WriteLine("Conversion completed.");

            Marshal.ReleaseComObject(objImage);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Conversion failed. Please check the input file and settings.");
            Console.WriteLine("Error code: 0x" + (ex.HResult & 0xFFFFFFFF).ToString("X8"));
        }
    }
}