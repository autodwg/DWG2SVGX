// ============================================================
// Go Example - DWG to SVG Conversion
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//   go mod tidy
//
// Run: go run example.go
// ============================================================

package main

import (
	"fmt"

	"github.com/go-ole/go-ole"
	"github.com/go-ole/go-ole/oleutil"
)

func main() {
	ole.CoInitialize(0)
	defer ole.CoUninitialize()

	unknown, err := oleutil.CreateObject("DWG2SVGX.DWG2SVG")
	if err != nil {
		fmt.Println("Failed to create COM object. Please make sure DWG2SVGX is registered.")
		fmt.Println("Error:", err)
		return
	}
	objImage, _ := unknown.QueryInterface(ole.IID_IDispatch)
	defer objImage.Release()

	// --- Basic Settings ---

	oleutil.CallMethod(objImage, "AddFontPath", `C:\fonts\`)
	oleutil.PutProperty(objImage, "ColorType", 0)    // 0=True Color, 1=B&W, 2=Gray
	oleutil.PutProperty(objImage, "LayoutType", 0)   // 0=All, 1=Model, 2=Paper
	oleutil.CallMethod(objImage, "SetSize", 800, 600)

	// --- Advanced Settings (optional) ---

	// oleutil.PutProperty(objImage, "OutputLayer", 0)
	// oleutil.PutProperty(objImage, "ConvertAsLayoutSizeOut", 0)
	// oleutil.PutProperty(objImage, "HatchType", 0)
	// oleutil.PutProperty(objImage, "Outputtype", 0)
	// oleutil.PutProperty(objImage, "ExplodeShxText", 0)
	// oleutil.PutProperty(objImage, "LineWeightScale", 1.0)
	// oleutil.PutProperty(objImage, "OutScale", 1.0)
	// oleutil.CallMethod(objImage, "SetConvertType", "model")

	// --- Convert ---
	_, err = oleutil.CallMethod(objImage, "Convert", `f:\2018.dwg`, `f:\`)
	if err != nil {
		fmt.Println("Conversion failed. Please check the input file and settings.")
		fmt.Println("Error:", err)
		return
	}

	fmt.Println("Conversion completed.")
}