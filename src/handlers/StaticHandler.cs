using System.Net;
using System.IO;
using System.Text;
using webserver.extensions;

namespace webserver.handlers {
  class StaticHandler : IRequestHandler {
    public byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming) {
      if (!ctx.Request.Url.LocalPath.ToLower().StartsWith("/static")
          & !ctx.Request.Url.LocalPath.ToLower().Equals("/")
          & !ctx.Request.Url.LocalPath.ToLower().EndsWith(".html")
          ) {
        // this is an API call
        return incoming;
      }
      string page = "";
      if (ctx.Request.Url.LocalPath.ToLower().Equals("/") || ctx.Request.Url.LocalPath.ToLower().EndsWith(".html")) {
        page = Directory.GetCurrentDirectory() + ctx.Request.Url.LocalPath.Replace("/", "/static/dist/index.html");
      } else {
        page = Directory.GetCurrentDirectory() + ctx.Request.Url.LocalPath.Replace("/", @"\");
      }
      // read from disk
      return File.ReadAllBytes(page);
    }
  }
}
