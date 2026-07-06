

 	Dim objImage
   	Set objImage= CreateObject("DWG2SVGX.DWG2SVG")
               objImage.AddFontPath "C:\\fonts\\"
              objImage.ColorType = 0  ' 0 true color 1  w/b 2 gray
        objImage.LayoutType = 0  ' 0 all layout 1 model sapce 2 paper space
        objImage.SetSize 800,600
	objImage.Convert "f:\\2018.dwg", "f:\\"

    	
	
 	
        


	