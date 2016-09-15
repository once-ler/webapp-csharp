using System.Net;
using System.IO;
using System.Text;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using webserver.extensions;
using webserver.contracts;

namespace webserver.handlers {
  class AuthHandler : IRequestHandler {
    public byte[] process(HttpListenerContext ctx, WebRequestInfo info, byte[] incoming) {
      if (!ctx.Request.Url.LocalPath.ToLower().Equals("/loadauth")
          & !ctx.Request.Url.LocalPath.ToLower().Equals("/login")
          & !ctx.Request.Url.LocalPath.ToLower().Equals("/logout")
          ) {
        return incoming;
      }

      dynamic response = new Dictionary<string, object>();

      response["user"] = new Dictionary<string, object> { { "id", "ANONYMOUS" }, { "firstName", "" }, { "lastName", "" } };
      response["headers"] = ctx.Request.Headers;
      response["cookies"] = ctx.Request.Cookies;
      
      // Ntlm
      if (ctx.User != null) {
        Dictionary<string, object> user = new Dictionary<string, object>();
        var domainUser = ctx.User.Identity.Name.Split('\\');
        user.Add("id", domainUser[1]);

        PrincipalContext pctx = new PrincipalContext(ContextType.Domain, domainUser[0]);
        UserPrincipal up = UserPrincipal.FindByIdentity(pctx, domainUser[1]);
        if (up != null) {
          user.Add("firstName", up.GivenName);
          user.Add("lastName", up.Surname);
          user.Add("email", up.EmailAddress);
        }
        response["user"] = user;
      }
      // var body = new BindableObjects<Dictionary<string, object>>(user).toJson();
      var body = Newtonsoft.Json.JsonConvert.SerializeObject(response);
      return Encoding.UTF8.GetBytes(body);
    }
  }
}
