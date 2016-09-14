using System.Net;
using System.IO;
using System.Text;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
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
      if (ctx.User == null) {
        return Encoding.UTF8.GetBytes("{\"firstName\": \"\",\"lastName\": \"\"}"); 
      }
      Dictionary<string, object> user = new Dictionary<string, object>();
      var domainUser = ctx.User.Identity.Name.Split('\\');
      user.Add("id", domainUser[1]);
      
      PrincipalContext pctx = new PrincipalContext(ContextType.Domain, domainUser[0]);
      UserPrincipal up = UserPrincipal.FindByIdentity(pctx, domainUser[1]);      
      if(up != null) {
        user.Add("firstName", up.GivenName);
        user.Add("lastName", up.Surname);
        user.Add("email", up.EmailAddress);
      }
      var body = new BindableObjects<Dictionary<string, object>>(user).toJson();
      return Encoding.UTF8.GetBytes(body);
    }
  }
}
