#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoDWG.DWG2SVG.Web.Services;

namespace AutoDWG.DWG2SVG.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DWGConverterService _service;
        public IndexModel(DWGConverterService service) { _service = service; }
        public ConversionResult? Result { get; set; }

        public void OnPost(IFormFile DwgFile, int ColorType, int LayoutType, int Width, int Height, string? FontPath,
            int OutputLayer, int HatchType, int OutputType, int ConvertAsLayoutSizeOut, int ExplodeShxText,
            double? LineWeightScale, double? OutScale, string? ConvertType)
        {
            if (DwgFile == null || DwgFile.Length == 0) return;
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(uploadDir);
            Directory.CreateDirectory(outputDir);
            var inputPath = Path.Combine(uploadDir, DwgFile.FileName);
            using (var stream = new FileStream(inputPath, FileMode.Create)) { DwgFile.CopyTo(stream); }

            var req = new ConversionRequest
            {
                ColorType = ColorType, LayoutType = LayoutType, Width = Width, Height = Height,
                FontPath = FontPath ?? @"C:\fonts\",
                OutputLayer = OutputLayer, HatchType = HatchType, OutputType = OutputType,
                ConvertAsLayoutSizeOut = ConvertAsLayoutSizeOut, ExplodeShxText = ExplodeShxText,
                LineWeightScale = LineWeightScale, OutScale = OutScale, ConvertType = ConvertType
            };
            Result = _service.Convert(inputPath, outputDir + "\\", req);
        }

        public IActionResult OnGetDownload(string file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "output", file);
            if (!System.IO.File.Exists(filePath)) return NotFound();
            return File(System.IO.File.ReadAllBytes(filePath), "image/svg+xml", file);
        }
    }
}