<?php
// ============================================================
// PHP Web - DWG to SVG Converter (Full API)
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//   PHP with COM extension: extension=com_dotnet in php.ini
//
// Run: php -S localhost:9879
// Browse: http://localhost:9879
// ============================================================
session_start();
$result = null;
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_FILES['dwg_file'])) {
    $file = $_FILES['dwg_file'];
    if ($file['error'] === UPLOAD_ERR_OK && strtolower(pathinfo($file['name'], PATHINFO_EXTENSION)) === 'dwg') {
        $uploadDir = __DIR__ . '/uploads/';
        $outputDir = __DIR__ . '/output/';
        if (!is_dir($uploadDir)) mkdir($uploadDir, 0777, true);
        if (!is_dir($outputDir)) mkdir($outputDir, 0777, true);
        $inputPath = $uploadDir . basename($file['name']);
        if (move_uploaded_file($file['tmp_name'], $inputPath)) {
            try {
                $obj = new COM("DWG2SVGX.DWG2SVG");
                // Basic
                $fontPath = trim($_POST['font_path'] ?? '');
                if ($fontPath) $obj->AddFontPath($fontPath);
                $obj->ColorType = (int)($_POST['color_type'] ?? 0);
                $obj->LayoutType = (int)($_POST['layout_type'] ?? 0);
                $obj->SetSize((int)($_POST['width'] ?? 800), (int)($_POST['height'] ?? 600));
                // Advanced
                $obj->OutputLayer = (int)($_POST['output_layer'] ?? 0);
                $obj->HatchType = (int)($_POST['hatch_type'] ?? 0);
                $obj->Outputtype = (int)($_POST['output_type'] ?? 0);
                $obj->ConvertAsLayoutSizeOut = isset($_POST['convert_as_layout_size']) ? 1 : 0;
                $obj->ExplodeShxText = isset($_POST['explode_shx_text']) ? 1 : 0;
                $lws = $_POST['line_weight_scale'] ?? '';
                if ($lws !== '') $obj->LineWeightScale = (float)$lws;
                $os = $_POST['out_scale'] ?? '';
                if ($os !== '') $obj->OutScale = (float)$os;
                $ct = trim($_POST['convert_type'] ?? '');
                if ($ct) $obj->SetConvertType($ct);

                $obj->Convert($inputPath, $outputDir);
                // SVG files are named as {dwgName}-{layoutName}.svg
                $dwgBase = pathinfo($inputPath, PATHINFO_FILENAME);
                $files = glob($outputDir . $dwgBase . '-*.svg');
                $files = array_map('basename', $files);
                $fileCount = count($files);
                $result = ['success' => true, 'message' => "Generated $fileCount SVG file(s).", 'files' => $files];
            } catch (Exception $e) {
                $result = ['success' => false, 'message' => $e->getMessage()];
            }
        }
    }
}
?>
<!DOCTYPE html><html lang="en"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1.0">
<title>DWG to SVG Converter</title>
<style>
*{margin:0;padding:0;box-sizing:border-box}body{font-family:'Segoe UI',sans-serif;background:#f0f2f5;color:#333}
.container{max-width:700px;margin:40px auto;padding:0 20px}.card{background:#fff;border-radius:12px;padding:25px;margin-bottom:20px;box-shadow:0 2px 8px rgba(0,0,0,.1)}
h1{text-align:center;color:#0078d4;margin-bottom:30px}h2{font-size:18px;color:#0078d4;margin-bottom:15px}
.form-group{margin-bottom:15px}label{display:block;margin-bottom:5px;font-weight:600;font-size:14px}
input[type=text],input[type=number],select{width:100%;padding:8px 12px;border:1px solid #ddd;border-radius:6px;font-size:14px}
fieldset{border:1px solid #e0e0e0;border-radius:8px;padding:15px;margin-bottom:10px}legend{font-weight:600;color:#555;padding:0 8px}
.form-row{display:grid;grid-template-columns:1fr 1fr 1fr;gap:12px}.checkbox-group{display:flex;gap:20px;margin-top:10px}
.btn{display:block;width:100%;padding:12px;background:linear-gradient(135deg,#0078d4,#005a9e);color:#fff;border:none;border-radius:8px;font-size:16px;font-weight:700;cursor:pointer}
.btn:hover{opacity:.9}.download-btn{display:inline-block;padding:8px 20px;background:linear-gradient(135deg,#28a745,#1e7e34);color:#fff;text-decoration:none;border-radius:6px;font-weight:700}
.result{padding:15px;border-radius:8px;margin-top:10px}.success{background:#d4edda;border:1px solid #28a745;color:#155724}.error{background:#f8d7da;border:1px solid #dc3545;color:#721c24}
</style></head><body><div class="container"><h1>DWG to SVG Converter</h1>
<form method="post" enctype="multipart/form-data">
<div class="card"><h2>1. Select DWG File</h2><div class="form-group"><label>Upload a DWG file:</label><input type="file" name="dwg_file" accept=".dwg" required></div></div>
<div class="card"><h2>2. Conversion Settings</h2>
<fieldset><legend>Basic</legend>
<div class="form-row"><div class="form-group"><label>Color Type</label><select name="color_type"><option value="0">0 - True Color</option><option value="1">1 - Black &amp; White</option><option value="2">2 - Gray</option></select></div>
<div class="form-group"><label>Layout Type</label><select name="layout_type"><option value="0">0 - All Layouts</option><option value="1">1 - Model Space</option><option value="2">2 - Paper Space</option></select></div></div>
<div class="form-row"><div class="form-group"><label>Width (px)</label><input type="number" name="width" value="800" min="100" max="10000"></div>
<div class="form-group"><label>Height (px)</label><input type="number" name="height" value="600" min="100" max="10000"></div></div>
<div class="form-group"><label>Font Path</label><input type="text" name="font_path" value="C:\fonts\"></div>
</fieldset>
<fieldset style="margin-top:15px"><legend>Advanced (optional)</legend>
<div class="form-row"><div class="form-group"><label>OutputLayer</label><input type="number" name="output_layer" value="0" min="0" max="10"></div>
<div class="form-group"><label>HatchType</label><input type="number" name="hatch_type" value="0" min="0" max="10"></div>
<div class="form-group"><label>OutputType</label><input type="number" name="output_type" value="0" min="0" max="10"></div></div>
<div class="form-row"><div class="form-group"><label>LineWeightScale</label><input type="number" name="line_weight_scale" step="0.1" placeholder="1.0"></div>
<div class="form-group"><label>OutScale</label><input type="number" name="out_scale" step="0.1" placeholder="1.0"></div>
<div class="form-group"><label>ConvertType</label><input type="text" name="convert_type" placeholder="e.g. model"></div></div>
<div class="checkbox-group"><label><input type="checkbox" name="convert_as_layout_size"> ConvertAsLayoutSizeOut</label>
<label><input type="checkbox" name="explode_shx_text"> ExplodeShxText</label></div>
</fieldset></div>
<div style="text-align:center;margin:20px 0"><button type="submit" class="btn">Convert to SVG</button></div></form>
<?php if ($result): ?>
<div class="card"><h2>3. Result</h2>
<?php if ($result['success']): ?>
<div class="result success"><p style="font-size:16px"><strong><?= htmlspecialchars($result['message']) ?></strong></p>
<?php foreach ($result['files'] as $f): ?><p style="margin:5px 0"><a href="output/<?= urlencode($f) ?>" class="download-btn" download>&#11015; <?= htmlspecialchars($f) ?></a></p><?php endforeach; ?>
</div><?php else: ?><div class="result error"><p><strong>Failed.</strong></p><p><?= htmlspecialchars($result['message']) ?></p></div><?php endif; ?>
</div>
<?php if ($result['success']): ?><script>alert("<?= htmlspecialchars($result['message']) ?>");</script><?php else: ?><script>alert("Conversion failed!\n\n<?= htmlspecialchars($result['message']) ?>");</script><?php endif; ?>
<?php endif; ?>
</div></body></html>