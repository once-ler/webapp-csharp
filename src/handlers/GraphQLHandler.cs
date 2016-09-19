using System.Net;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;

using webserver.extensions;

namespace webserver.handlers {
  class GraphQLHandler : IRequestHandler {

    public class GraphQLQuery {
      public string OperationName { get; set; }
      public string NamedQuery { get; set; }
      public string Query { get; set; }
      public string Variables { get; set; }
    }

    private readonly ISchema _schema;
    private readonly IDocumentExecuter _executer = new DocumentExecuter();
    private readonly IDocumentWriter _writer = new DocumentWriter();
    
    public GraphQLHandler(ISchema schema) {
      _schema = schema;
    }

    public async Task<ExecutionResult> ExecuteAsync(ISchema schema, object rootObject, string query, string operationName = null, Inputs inputs = null) {
      return await _executer.ExecuteAsync(schema, rootObject, query, operationName, inputs).ConfigureAwait(true);
    }

    public byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming) {
      if (!ctx.Request.Url.LocalPath.ToLower().Equals("/graphql")) {
        return incoming;
      }

      string response = "{ \"message\": \"Error Ocurred\"}"; 
      var query = new GraphQLQuery { Query = info.body, Variables = "" };
      var runner = ExecuteAsync(_schema, null, query.Query, query.OperationName, query.Variables.ToInputs());
      var result = runner.Result;
  
      if (result.Errors.Count == 0) {
        response = _writer.Write(result);        
      }

      return Encoding.UTF8.GetBytes(response);
    }    
  }
}
