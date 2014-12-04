using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace AGM.Web.Infrastructure.Attributes
{
    public class AuthorizeActionAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                var headers = actionContext.Request.Headers;
                var sessionId = headers.GetCookieValue("SSID");
                var headerSessionId = headers.GetHeaderValue("SSID");
                var tokenEnc = headers.GetCookieValue("SSTKN");
                var headerTokenEnc = headers.GetCookieValue("SSTKN");

                if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(tokenEnc) || sessionId != headerSessionId || tokenEnc != headerTokenEnc )
                    throw new OperationException(System.Net.HttpStatusCode.BadRequest, "Sessione non autenticata");

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(tokenEnc) as JwtSecurityToken;
                if (token == null || token.Claims.All(c => c.Type != "unique_name"))
                    throw new OperationException(System.Net.HttpStatusCode.BadRequest, "Sessione non autenticata");

                Thread.CurrentPrincipal = new CustomPrincipal(token.Claims.Where(c => c.Type == "unique_name").First().Value);
                Users = token.Claims.Where(c => c.Type == "unique_name").First().Value;
            }
            catch (Exception ex)
            {
                HttpStatusCode status = (ex is OperationException)?  ((OperationException)ex).HttpStatus : HttpStatusCode.InternalServerError;
                actionContext.Response = new HttpResponseMessage(status)
                {
                    Content = new StringContent(ex.Message)
                };
            }
        }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }
    }
}