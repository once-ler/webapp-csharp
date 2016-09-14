using System.Net;
using System.Text;

namespace webserver.extensions {
  public static class HttpListenerResponseExtensions {
    public static void WriteContent(this HttpListenerResponse response, byte[] buffer) {
      try {
        // byte[] buffer = Encoding.UTF8.GetBytes(body);
        if (response.StatusCode != 302) {
          response.ContentLength64 = buffer.Length;
          response.OutputStream.Write(buffer, 0, buffer.Length);
          response.OutputStream.Flush();
          response.OutputStream.Close();
        }
      } catch (HttpListenerException) {
        // if the client disconnect while we are sending the response, a HttpListenerException will be thrown
      }
    }
  }
}
