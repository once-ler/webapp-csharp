using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using webserver.handlers;
using webserver.extensions; //extends the HttpListenerResponse Class

namespace webserver.host {
  
  public sealed class AsyncHttpListener : IDisposable {
    private List<IRequestHandler> handlers = new List<IRequestHandler>();
    private System.Net.HttpListener _httpListener;
    private Dictionary<string, System.Net.AuthenticationSchemes> authTypes = new Dictionary<string,AuthenticationSchemes>{ 
      {"NTLM", AuthenticationSchemes.Ntlm },
      { "ANONYMOUS", AuthenticationSchemes.Anonymous } 
    };

    public AsyncHttpListener(int port, List<IRequestHandler> customHandlers) {
      // default => static file handler (ie html/css/etc)
      handlers.Add(new StaticHandler());
      // Add Windows Auth handler
      handlers.Add(new AuthHandler());
      // add customHandlers
      customHandlers.ForEach(h => handlers.Add(h));
      // actually create the listener object
      _httpListener = new HttpListener();
      // netsh http add urlacl url=http://+:7777/ user=everyone
      _httpListener.Prefixes.Add("http://+:" + port.ToString() + "/");
      // use Windows authentication
      _httpListener.AuthenticationSchemes = authTypes[webapp_csharp.Properties.Settings.Default.AuthenticationType];
      _httpListener.UnsafeConnectionNtlmAuthentication = true;
      
      Console.WriteLine("Listening for requests on http://localhost:7777/");
    }
    
    public void Dispose() {
      Stop();
    }
    
    public void Start() {
      _httpListener.Start();

      while (_httpListener.IsListening)
        ProcessRequest();
    }
    
    public void Stop() {
      _httpListener.Stop();
    }
    
    void ProcessRequest() {
      var result = _httpListener.BeginGetContext(ListenerCallback, _httpListener);
      result.AsyncWaitHandle.WaitOne();
    }

    void ListenerCallback(IAsyncResult result) {
      var context = _httpListener.EndGetContext(result);
      var info = readRequest(context.Request);
      byte[] buffer = Encoding.UTF8.GetBytes("Not Found");

      try {
        // your custom handler will be processed here
        handlers.ForEach(h => {
          buffer = h.process(context, info, buffer);
        });
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.StatusDescription = HttpStatusCode.OK.ToString();
      } catch (Exception e) {
        buffer = Encoding.UTF8.GetBytes(e.Message);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.StatusDescription = HttpStatusCode.InternalServerError.ToString();
      }
      context.Response.WriteContent(buffer);      
    }
      
    public static WebRequestInfo readRequest(HttpListenerRequest request) {
      var info = new WebRequestInfo();
      info.httpMethod = request.HttpMethod;
      info.url = request.Url;

      if (request.HasEntityBody) {
        Encoding encoding = request.ContentEncoding;
        using (var bodyStream = request.InputStream)
        using (var streamReader = new StreamReader(bodyStream, encoding)) {
          if (request.ContentType != null)
            info.contentType = request.ContentType;

          info.contentLength = request.ContentLength64;
          info.body = streamReader.ReadToEnd();
        }
      }       
      return info;
    }

  }
}
