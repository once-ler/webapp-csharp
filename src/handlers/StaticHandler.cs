using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using webserver.extensions;

namespace webserver.handlers {
  class StaticHandler : IRequestHandler {
    static Regex graphiqlEndingPathMatch = new Regex(@"\/(graphiql)\/?", RegexOptions.IgnoreCase);
    static Regex utf8Match = new Regex(@"css|html|xml|script$", RegexOptions.IgnoreCase);

    public byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming) {
      if (!ctx.Request.Url.LocalPath.ToLower().StartsWith("/static")
          & !ctx.Request.Url.LocalPath.ToLower().Equals("/")
          & !ctx.Request.Url.LocalPath.ToLower().EndsWith(".html")
          & !graphiqlEndingPathMatch.IsMatch(ctx.Request.Url.LocalPath)
          ) {
        // this is an API call
        return incoming;
      }
      string page = "";
      if (ctx.Request.Url.LocalPath.ToLower().Equals("/") || ctx.Request.Url.LocalPath.ToLower().EndsWith(".html")) {
        page = Directory.GetCurrentDirectory() + ctx.Request.Url.LocalPath.Replace("/", "/static/dist/index.html");
      } else {
        page = Directory.GetCurrentDirectory() + "/static" + ctx.Request.Url.LocalPath.Replace("/", @"\");
        // if path is a directory, append index.html as default behavior
        page = Directory.Exists(page) ? page + @"\index.html" : page;
      }
      // read from disk
      byte[] buffer = File.ReadAllBytes(page);

      // add mime type
      ctx.Response.ContentType = page.GetMimeType();
      ctx.Response.ContentLength64 = buffer.Length;
      ctx.Response.AddHeader("Date", DateTime.Now.ToString("r"));
      ctx.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(page).ToString("r"));

      return buffer;
    }
  }
}
