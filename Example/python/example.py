# ============================================================
# Python Example - DWG to SVG Conversion
#
# Prerequisites:
#   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
#   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
#   pip install pywin32
#
# Run: python example.py
# ============================================================

import win32com.client

try:
    obj = win32com.client.Dispatch("DWG2SVGX.DWG2SVG")

    # --- Basic Settings ---

    # Add font search path
    obj.AddFontPath(r"C:\fonts\")

    # Color mode: 0 = True Color, 1 = Black & White, 2 = Gray
    obj.ColorType = 0

    # Layout: 0 = All Layouts, 1 = Model Space, 2 = Paper Space
    obj.LayoutType = 0

    # Output size in pixels
    obj.SetSize(800, 600)

    # --- Advanced Settings (optional) ---

    # obj.OutputLayer = 0
    # obj.ConvertAsLayoutSizeOut = 0
    # obj.HatchType = 0
    # obj.Outputtype = 0
    # obj.ExplodeShxText = 0
    # obj.LineWeightScale = 1.0
    # obj.OutScale = 1.0
    # obj.SetConvertType("model")

    # --- Convert ---
    obj.Convert(r"f:\2018.dwg", r"f:\")

    print("Conversion completed.")

except Exception as e:
    print("Conversion failed. Please check the input file and settings.")
    if hasattr(e, 'hresult'):
        print(f"Error code: {hex(e.hresult)}")
    else:
        print(f"Error: {e}")