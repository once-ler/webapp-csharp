using System;
using System.Text;

namespace webserver.extensions {
  public class WebRequestInfo {
    public string body { get; set; }
    public long contentLength { get; set; }
    public string contentType { get; set; }
    public string httpMethod { get; set; }
    public Uri url { get; set; }

    public override string ToString() {
      var sb = new StringBuilder();
      sb.AppendLine(string.Format("HttpMethod {0}", httpMethod));
      sb.AppendLine(string.Format("Url {0}", url));
      sb.AppendLine(string.Format("ContentType {0}", contentType));
      sb.AppendLine(string.Format("ContentLength {0}", contentLength));
      sb.AppendLine(string.Format("Body {0}", body));
      return sb.ToString();
    }
  }
}
