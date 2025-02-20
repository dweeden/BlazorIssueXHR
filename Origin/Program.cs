using System.Net;

Environment.SetEnvironmentVariable("DOTNET_hostBuilder:reloadConfigOnChange", "false");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
    Results.Text(contentType: "text/html",
        content: "<ul><li><a href='QueryToken'>Token in Query String</a></li><li><a href='HeaderToken1?foo=bar'>Token in Header 1</a></li><li><a href='HeaderToken2?foo=bar'>Token in Header 2</a></li><ul>"
    )
);

app.MapGet("/QueryToken", () =>
    Results.Text(contentType: "text/html",
        content: "<iframe width='1024' height='768' src='http://localhost:5062/?token=SampleQueryTokenValue'></iframe>"
    )
);

app.MapGet("/HeaderToken1", () =>
    Results.Text(contentType: "text/html",
        content: "<iframe width='1024' height='768'></iframe>\n" +
                 "     <script>\n" +
                 "       async function getSrc() {\n" +
                 "         const res = await fetch(\"http://localhost:5062\", {\n" +
                 "           method: 'GET',\n" +
                 "           headers: {\n" +
                 "             // Here you can set any headers you want\n" +
                 "           }\n" +
                 "         });\n" +
                 "         const blob = await res.blob();\n" +
                 "         const urlObject = URL.createObjectURL(blob);\n" +
                 "         document.querySelector('iframe').setAttribute(\"src\", urlObject)\n" +
                 "       }\n" +
                 "       getSrc();\n" +
                 "   </script>"
    )
);

app.MapGet("/HeaderToken2", () =>
    Results.Text(contentType: "text/html",
        content: "<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js'></script>\r\n" +
                 "<script>\r\n" +
                 "  function populateIframe(iframe, url, token) {\r\n" +
                    
                 "    var headers = [['Authorization', 'Bearer ' + token], ['Access-Control-Allow-Origin']];\r\n" +
                    
                 "    var xhr = new XMLHttpRequest();\r\n" +
                 "    xhr.open('GET', url);\r\n" +
                 "    xhr.withCredentials = true;\r\n" +
                 "    xhr.onreadystatechange = handler;\r\n" +
                 "    xhr.responseType = 'document';\r\n" +
                 "    headers.forEach(function (header) {\r\n" +
                 "        xhr.setRequestHeader(header[0], header[1]);\r\n" +
                 "    });\r\n" +
                 "    xhr.send();\r\n" +
                    
                 "    function handler() {\r\n" +
                 "        if (this.readyState === this.DONE) {\r\n" +
                 "            if (this.status === 200) {\r\n" +
                 "                var content = iframe[0].contentWindow ||\r\n" +
                 "                    iframe[0].contentDocument.document || \r\n" +
                 "                    iframe[0].contentDocument;\r\n" +
                 "                content.document.open();\r\n" +
                 "                content.document.write(this.response.documentElement.innerHTML);\r\n" +
                 "                content.document.close();\r\n" +
                 "            } else {\r\n" +
                 "                alert(this.status);\r\n" +
                 "                iframe.attr('srcdoc', 'Error loading page');\r\n" +
                 "            }\r\n" +
                 "        }\r\n" +
                 "    }\r\n" +
                 "  }\r\n" +
                    
                 "  window.$j = jQuery.noConflict();\r\n" +
                 "  $j(document).ready(function() {\r\n" +
                 "      populateIframe($j('#HeaderTokenIFrame'), 'http://localhost:5062', 'SampleBearerTokenValue');\r\n" +
                 "  });\r\n" +
                 
                 "</script>\r\n" +
                 
                 "<iframe id='HeaderTokenIFrame' width='1024' height='768'></iframe>"
    )
);

/*
  
 */

app.Run();