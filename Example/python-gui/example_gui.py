# ============================================================
# Python GUI - DWG to SVG Converter (Full API)
#
# Prerequisites:
#   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
#   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
#   pip install pywin32
#
# Run: python example_gui.py
# ============================================================

import tkinter as tk
from tkinter import ttk, filedialog, messagebox, scrolledtext
import win32com.client
import os, datetime


class DWG2SVGApp:
    def __init__(self, root):
        self.root = root
        self.root.title("AutoDWG DWG to SVG Converter")
        self.root.geometry("600x650")
        self.root.resizable(False, False)

        y = 10

        # Input
        tk.Label(root, text="Input DWG:").place(x=10, y=y+3)
        self.txt_input = tk.Entry(root, width=42); self.txt_input.place(x=100, y=y)
        tk.Button(root, text="...", width=3, command=self.browse_input).place(x=470, y=y)
        y += 30

        # Output
        tk.Label(root, text="Output Path:").place(x=10, y=y+3)
        self.txt_output = tk.Entry(root, width=42); self.txt_output.place(x=100, y=y)
        self.txt_output.insert(0, r"f:\")
        tk.Button(root, text="...", width=3, command=self.browse_output).place(x=470, y=y)
        y += 35

        # Basic settings
        grp_basic = ttk.LabelFrame(root, text="Basic Settings")
        grp_basic.place(x=10, y=y, width=570, height=90)

        tk.Label(grp_basic, text="Color Type:").place(x=10, y=18)
        self.cmb_color = tk.StringVar(value="0 - True Color")
        tk.OptionMenu(grp_basic, self.cmb_color, "0 - True Color", "1 - Black & White", "2 - Gray").place(x=85, y=15, width=140)

        tk.Label(grp_basic, text="Layout Type:").place(x=240, y=18)
        self.cmb_layout = tk.StringVar(value="0 - All Layouts")
        tk.OptionMenu(grp_basic, self.cmb_layout, "0 - All Layouts", "1 - Model Space", "2 - Paper Space").place(x=320, y=15, width=140)

        tk.Label(grp_basic, text="W:").place(x=10, y=50)
        self.nud_w = tk.Spinbox(grp_basic, from_=100, to=10000, width=7); self.nud_w.delete(0, tk.END); self.nud_w.insert(0, "800"); self.nud_w.place(x=30, y=50)
        tk.Label(grp_basic, text="H:").place(x=100, y=50)
        self.nud_h = tk.Spinbox(grp_basic, from_=100, to=10000, width=7); self.nud_h.delete(0, tk.END); self.nud_h.insert(0, "600"); self.nud_h.place(x=120, y=50)

        tk.Label(grp_basic, text="Font Path:").place(x=240, y=50)
        self.txt_font = tk.Entry(grp_basic, width=22); self.txt_font.place(x=315, y=50)
        self.txt_font.insert(0, r"C:\fonts\")
        y += 100

        # Advanced settings
        grp_adv = ttk.LabelFrame(root, text="Advanced Settings (optional)")
        grp_adv.place(x=10, y=y, width=570, height=120)

        tk.Label(grp_adv, text="OutputLayer:").place(x=10, y=18)
        self.nud_output_layer = tk.Spinbox(grp_adv, from_=0, to=10, width=5); self.nud_output_layer.delete(0, tk.END); self.nud_output_layer.insert(0, "0"); self.nud_output_layer.place(x=95, y=18)
        tk.Label(grp_adv, text="HatchType:").place(x=150, y=18)
        self.nud_hatch = tk.Spinbox(grp_adv, from_=0, to=10, width=5); self.nud_hatch.delete(0, tk.END); self.nud_hatch.insert(0, "0"); self.nud_hatch.place(x=225, y=18)
        tk.Label(grp_adv, text="OutputType:").place(x=280, y=18)
        self.nud_outtype = tk.Spinbox(grp_adv, from_=0, to=10, width=5); self.nud_outtype.delete(0, tk.END); self.nud_outtype.insert(0, "0"); self.nud_outtype.place(x=360, y=18)

        tk.Label(grp_adv, text="LineWeightScale:").place(x=10, y=48)
        self.txt_lws = tk.Entry(grp_adv, width=8); self.txt_lws.place(x=120, y=48); self.txt_lws.insert(0, "1.0")
        tk.Label(grp_adv, text="OutScale:").place(x=200, y=48)
        self.txt_outscale = tk.Entry(grp_adv, width=8); self.txt_outscale.place(x=270, y=48); self.txt_outscale.insert(0, "1.0")
        tk.Label(grp_adv, text="ConvertType:").place(x=340, y=48)
        self.txt_convtype = tk.Entry(grp_adv, width=12); self.txt_convtype.place(x=425, y=48)

        self.chk_layout_size = tk.BooleanVar()
        tk.Checkbutton(grp_adv, text="ConvertAsLayoutSizeOut", variable=self.chk_layout_size).place(x=10, y=78)
        self.chk_explode_shx = tk.BooleanVar()
        tk.Checkbutton(grp_adv, text="ExplodeShxText", variable=self.chk_explode_shx).place(x=200, y=78)
        y += 130

        # Convert
        self.btn = tk.Button(root, text="Convert to SVG", bg="#0078d4", fg="white", font=("Segoe UI", 11, "bold"), width=18, command=self.do_convert)
        self.btn.place(x=180, y=y)
        y += 40

        # Status
        self.lbl_status = tk.Label(root, text="Ready", fg="gray"); self.lbl_status.place(x=10, y=y)
        y += 25

        # Log
        self.txt_log = scrolledtext.ScrolledText(root, width=70, height=6, state=tk.DISABLED, bg="#f5f5f5")
        self.txt_log.place(x=10, y=y)

    def browse_input(self):
        p = filedialog.askopenfilename(filetypes=[("DWG", "*.dwg")])
        if p: self.txt_input.delete(0, tk.END); self.txt_input.insert(0, p)

    def browse_output(self):
        p = filedialog.askdirectory()
        if p: self.txt_output.delete(0, tk.END); self.txt_output.insert(0, p + "\\")

    def log(self, msg):
        self.txt_log.config(state=tk.NORMAL)
        self.txt_log.insert(tk.END, f"[{datetime.datetime.now():%H:%M:%S}] {msg}\n")
        self.txt_log.see(tk.END)
        self.txt_log.config(state=tk.DISABLED)

    def do_convert(self):
        inp = self.txt_input.get().strip()
        out = self.txt_output.get().strip()
        if not inp or not os.path.isfile(inp):
            messagebox.showwarning("Error", "Please select a valid DWG file."); return

        self.btn.config(state=tk.DISABLED)
        self.lbl_status.config(text="Converting...", fg="blue")
        self.log("Starting conversion...")

        try:
            obj = win32com.client.Dispatch("DWG2SVGX.DWG2SVG")

            # Basic
            fp = self.txt_font.get().strip()
            if fp: obj.AddFontPath(fp)
            obj.ColorType = int(self.cmb_color.get().split(" ")[0])
            obj.LayoutType = int(self.cmb_layout.get().split(" ")[0])
            obj.SetSize(int(self.nud_w.get()), int(self.nud_h.get()))

            # Advanced
            obj.OutputLayer = int(self.nud_output_layer.get())
            obj.HatchType = int(self.nud_hatch.get())
            obj.Outputtype = int(self.nud_outtype.get())
            obj.ConvertAsLayoutSizeOut = 1 if self.chk_layout_size.get() else 0
            obj.ExplodeShxText = 1 if self.chk_explode_shx.get() else 0
            try: obj.LineWeightScale = float(self.txt_lws.get())
            except: pass
            try: obj.OutScale = float(self.txt_outscale.get())
            except: pass
            ct = self.txt_convtype.get().strip()
            if ct: obj.SetConvertType(ct)

            obj.Convert(inp, out)

            # Scan output directory for generated SVG files (named as {dwgName}-{layoutName}.svg)
            import glob
            dwg_base = os.path.splitext(os.path.basename(inp))[0]
            svg_files = glob.glob(os.path.join(out, dwg_base + "-*.svg"))
            file_count = len(svg_files)
            self.log(f"Conversion completed. Generated {file_count} SVG file(s).")
            for sf in svg_files:
                self.log(f"  Generated: {os.path.basename(sf)}")

            self.lbl_status.config(text="Done!", fg="green")
            messagebox.showinfo("Success", f"Conversion completed!\n\nGenerated {file_count} SVG file(s).\n\nOutput: {out}")
        except Exception as e:
            self.log(f"ERROR: {hex(e.hresult) if hasattr(e, 'hresult') else str(e)}")
            self.lbl_status.config(text="Error!", fg="red")
            messagebox.showerror("Error", f"Conversion failed. Please check the input file and settings.\n\nError code: {hex(e.hresult) if hasattr(e, 'hresult') else str(e)}")
        finally:
            self.btn.config(state=tk.NORMAL)


if __name__ == "__main__":
    root = tk.Tk()
    DWG2SVGApp(root)
    root.mainloop()