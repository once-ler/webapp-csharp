# webapp-csharp

####An Opinionated C# Web App (No MVC)

```javascript
webserver
-src
  -> contracts
     -> BindableObjects
  -> extensions
     -> httpListenerResponseExtensions
     -> webRequestInfo
  -> handlers
     IRequestHandler (Interface must be implemented by all handlers)
     Simply define a function "process" with signature: byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming)
     -> AuthHandler (always called)
     -> StaticHandler (always called)
     -> LogHandler (always called)
  -> host
     -> asyncServer (default)
-> static
  -> dist
     any html/js/css  
```