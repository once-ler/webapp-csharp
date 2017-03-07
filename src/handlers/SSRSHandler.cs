using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webserver.extensions;
using webapp_csharp.ReportService2005;

namespace webserver.handlers {
  class SSRSHandler : IRequestHandler {
    protected static char pathSeparator = '/';
    protected static char[] pathSeparatorArray = { pathSeparator };
    protected static string pathSeparatorString = new string(pathSeparator, 1);
    protected static string reportPathRoot = "/";

    static Regex endingPathMatch = new Regex(@"\/(reports)\/?", RegexOptions.IgnoreCase);

    private JArray buildTree(CatalogItem[] catalogItems) {
      var nodesList = new JArray();
      foreach (CatalogItem item in catalogItems) {
        JArray nodes = new JArray();
        if (item.Type == ItemTypeEnum.Report) {
          string path = item.Path.Remove(0, reportPathRoot.Length);
          string[] tokens = path.Split(pathSeparatorArray);
          buildNodesFromTokens(tokens, 0, nodes);

          foreach (var child in nodes) {
            nodesList.Add(child);
          }
        }
      }
      return nodesList;
    }

    private void buildNodesFromTokens(string[] tokens, int index, JArray nodes) {
      JObject node = null;
      
      for (int i = 0; i < nodes.Count(); i++) {
        if ((string)nodes[i]["text"] == tokens[index]) {
          node = (JObject)nodes[i];
          break;
        }
      }
      if (node == null) {
        node = new JObject();
        node["text"] = tokens[index];
        nodes.Add(node);

        // Final token represents a report name
        if (tokens.Length - 1 == index) {          
          node["navigateUrl"] = "RenderReport.aspx?Path=" +
            System.Uri.EscapeUriString(string.Join(pathSeparatorString.ToString(), tokens));
        }
      }

      index++;
      if (tokens.Length > index) {
        node["children"] = new JArray();
        buildNodesFromTokens(tokens, index, (JArray)node["children"]);
      }
    }

    public byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming) {
      if (!endingPathMatch.IsMatch(ctx.Request.Url.LocalPath)) {
        return incoming;
      }
      string response = "{ \"message\": \"Error Ocurred\"}";

      try {
        ReportingService2005 rs = new ReportingService2005();
        rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
        CatalogItem[] catalogItems;
        catalogItems = rs.ListChildren(reportPathRoot, true);
        var nodes = buildTree(catalogItems);
        response = JsonConvert.SerializeObject(nodes);
      } catch (Exception e) {
        response = e.Message;
      }

      return Encoding.UTF8.GetBytes(response);
    }
  }
}
