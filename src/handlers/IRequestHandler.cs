using System.Net;
using webserver.extensions;

namespace webserver.handlers {
  public interface IRequestHandler {
    byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming);
  }
}
