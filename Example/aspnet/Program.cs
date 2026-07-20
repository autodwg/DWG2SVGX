using System.Diagnostics;
using System.Runtime.InteropServices;
using AutoDWG.DWG2SVG.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:9878");
builder.Services.AddRazorPages();
builder.Services.AddSingleton<DWGConverterService>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStarted.Register(() =>
{
    Task.Delay(500).ContinueWith(_ =>
    {
        try { Process.Start(new ProcessStartInfo { FileName = "http://localhost:9878", UseShellExecute = true }); }
        catch { }
    });
});
app.Run();

namespace AutoDWG.DWG2SVG.Web.Services
{
    public class DWGConverterService
    {
        public ConversionResult Convert(string inputPath, string outputDir, ConversionRequest req)
        {
            try
            {
                Type comType = Type.GetTypeFromProgID("DWG2SVGX.DWG2SVG")!;
                dynamic obj = Activator.CreateInstance(comType);

                if (!string.IsNullOrEmpty(req.FontPath)) obj.AddFontPath(req.FontPath);
                obj.ColorType = req.ColorType;
                obj.LayoutType = req.LayoutType;
                obj.SetSize(req.Width, req.Height);

                // Advanced
                obj.OutputLayer = (short)req.OutputLayer;
                obj.HatchType = (short)req.HatchType;
                obj.Outputtype = (short)req.OutputType;
                obj.ConvertAsLayoutSizeOut = (short)req.ConvertAsLayoutSizeOut;
                obj.ExplodeShxText = (short)req.ExplodeShxText;
                if (req.LineWeightScale.HasValue) obj.LineWeightScale = req.LineWeightScale.Value;
                if (req.OutScale.HasValue) obj.OutScale = (float)req.OutScale.Value;
                if (!string.IsNullOrEmpty(req.ConvertType)) obj.SetConvertType(req.ConvertType);

                obj.Convert(inputPath, outputDir);

                // Get output files - SVG files are named as {dwgName}-{layoutName}.svg
                Marshal.ReleaseComObject(obj);
                string dwgBaseName = Path.GetFileNameWithoutExtension(inputPath);
                var svgFiles = Directory.GetFiles(outputDir, dwgBaseName + "-*.svg")
                    .Select(Path.GetFileName)
                    .Where(n => n != null)
                    .Select(n => n!)
                    .OrderBy(n => n)
                    .ToList();
                short fileCount = (short)svgFiles.Count;

                return new ConversionResult { Success = true, OutputFiles = svgFiles, FileCount = fileCount, Message = $"Conversion completed! Generated {fileCount} SVG file(s)." };
            }
            catch (Exception ex)
            {
                return new ConversionResult { Success = false, Message = "Conversion failed. Please check the input file and settings, then try again. (Error: 0x" + (ex.HResult & 0xFFFFFFFF).ToString("X8") + ")" };
            }
        }
    }

    public class ConversionRequest
    {
        // Basic
        public int ColorType { get; set; } = 0;
        public int LayoutType { get; set; } = 0;
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public string? FontPath { get; set; }
        // Advanced
        public int OutputLayer { get; set; } = 0;
        public int HatchType { get; set; } = 0;
        public int OutputType { get; set; } = 0;
        public int ConvertAsLayoutSizeOut { get; set; } = 0;
        public int ExplodeShxText { get; set; } = 0;
        public double? LineWeightScale { get; set; }
        public double? OutScale { get; set; }
        public string? ConvertType { get; set; }
    }

    public class ConversionResult
    {
        public bool Success { get; set; }
        public List<string> OutputFiles { get; set; } = new();
        public short FileCount { get; set; }
        public string Message { get; set; } = "";
    }
}