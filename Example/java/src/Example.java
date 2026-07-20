// ============================================================
// Java Example - DWG to SVG Conversion
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//   Jacob library: https://github.com/freemansoft/jacob-project
//
// Build: javac -cp "lib\jacob.jar" src\Example.java -d out
// Run:   java -cp "out;lib\jacob.jar" -Djava.library.path="C:\Windows\System32" Example
// ============================================================

import com.jacob.activeX.ActiveXComponent;
import com.jacob.com.ComThread;

public class Example {
    public static void main(String[] args) {
        ComThread.InitSTA();
        try {
            ActiveXComponent objImage = new ActiveXComponent("DWG2SVGX.DWG2SVG");

            // --- Basic Settings ---

            objImage.invoke("AddFontPath", "C:\\fonts\\");
            objImage.setProperty("ColorType", 0);     // 0=True Color, 1=B&W, 2=Gray
            objImage.setProperty("LayoutType", 0);    // 0=All, 1=Model, 2=Paper
            objImage.invoke("SetSize", 800, 600);

            // --- Advanced Settings (optional) ---

            // objImage.setProperty("OutputLayer", 0);
            // objImage.setProperty("ConvertAsLayoutSizeOut", 0);
            // objImage.setProperty("HatchType", 0);
            // objImage.setProperty("Outputtype", 0);
            // objImage.setProperty("ExplodeShxText", 0);
            // objImage.setProperty("LineWeightScale", 1.0);
            // objImage.setProperty("OutScale", 1.0f);
            // objImage.invoke("SetConvertType", "model");

            // --- Convert ---
            objImage.invoke("Convert", "f:\\2018.dwg", "f:\\");

            System.out.println("Conversion completed.");

            objImage.safeRelease();
        } catch (Exception e) {
            System.out.println("Conversion failed. Please check the input file and settings.");
            System.out.println("Error: " + e.getMessage());
        } finally {
            ComThread.Release();
        }
    }
}