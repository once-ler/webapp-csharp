# animalcensus

####An Opinionated C# Web App for SQL Experts (No MVC)

```javascript
factories
  purpose: Some abstraction to any data source that will always implement 2 functions that can return IList<Dictionary<string, object>> for detail data and int datatype for counts.
  -> data
     -> SqlHelper (SQL Server)
        -> IList<Dictionary<string, object>> getSqlResults(string connString, string sqlString)
        -> int getSqlCount(string connString, string sqlString, string fieldName)
     -> PQHelper (Postgresql)
     -> CsvHelper
     -> XmlHelper
     -> ...
webserver
  purpose: 
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
     -> server (for debugging)
  -> static
     -> dist
        any html/js/css
```