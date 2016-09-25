using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Newtonsoft.Json;

using webserver.extensions;

namespace webserver.handlers {
  class GraphQLHandler : IRequestHandler {

    static Regex endingPathMatch = new Regex(@"\/(graphql)\/?", RegexOptions.IgnoreCase);

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
      if (!endingPathMatch.IsMatch(ctx.Request.Url.LocalPath)) {
        return incoming;
      }
      string response = "{ \"message\": \"Error Ocurred\"}";
      var query = ctx.Request.QueryString["query"] != null ? ctx.Request.QueryString["query"] : info.body;
      
      var gql = new GraphQLQuery { Query = query, Variables = "" };
      Task<ExecutionResult> runner;
      try {
        runner = ExecuteAsync(_schema, null, gql.Query, gql.OperationName, gql.Variables.ToInputs());
        // runner = ExecuteAsync(_schema, null, gql.Query);
        var result = runner.Result;

        if (result.Data != null) {
          response = _writer.Write(result);
        }
        
        if (result.Errors != null && result.Errors.Count > 0) {
          response = string.Join(",", result.Errors.Map(d => d.ToString()));
        }
      } catch (Exception e) {
        response = e.Message;
      }

      return Encoding.UTF8.GetBytes(response);
    }    
  }
}
