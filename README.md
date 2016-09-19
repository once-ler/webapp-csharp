# webapp-csharp

####An Opinionated C# Web App (No MVC) with support for GraphQL

```javascript
webserver
-src
  -> extensions
     -> httpListenerResponseExtensions
     -> webRequestInfo
  -> handlers
     IRequestHandler (Interface must be implemented by all handlers)
     Simply define a function "process" with signature: byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming)
     -> AuthHandler (always called)
     -> StaticHandler (always called)
     -> LogHandler (always called __Not yet implemented__)
     -> GraphQLHandler (optional)
  -> host
     -> asyncServer (default)
-> static
  -> dist
     any html/js/css  
```