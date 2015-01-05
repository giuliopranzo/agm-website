using AGM.Web.Infrastructure;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;

namespace AGM.Web.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        public ApiResponse Login(dynamic loginData)
        {
            string username = loginData.Username;
            string password = loginData.Password;
            string name = string.Empty;

            if (ConfigurationHelper.UseMockupData)
            {
                using (var re = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/App/Mockup/users.js")))
                {
                    JsonTextReader reader = new JsonTextReader(re);
                    JsonSerializer se = new JsonSerializer();
                    var parsedData = se.Deserialize<List<User>>(reader);
                    if (parsedData.All(u => u.Username.ToLower() != username.ToLower() || u.Password != password))
                        return new ApiResponse(false)
                        {
                            Errors =
                                new ApiResponseError[] { new ApiResponseError() { Message = "Nome utente o password errati" } }
                        };
                }
            }
            else
            {
                using (var context = new AgmDataContext())
                {
                    if (context.Users.All(u => u.Username.ToLower() != username.ToLower() || u.Password != password || u._sectionMonthlyReportsVisible != 1))
                        return new ApiResponse(false)
                        {
                            Errors =
                                new ApiResponseError[]
                                {new ApiResponseError() {Message = "Nome utente o password errati"}}
                        };

                    name = context.Users.First(u => u.Username.ToLower() == username.ToLower() && u.Password == password).Name;
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, string.Format("{0}${1}", loginData.Username.ToString(), name))
            };

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = "Agm"
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            return new ApiResponse()
            {
                Succeed = true,
                Token = tokenHandler.WriteToken(jwtToken)
            };
        }

        [HttpGet]
        public ApiResponse IsAuthenticated()
        {
            if (!Request.Headers.Contains("AuthToken"))
                return new ApiResponse()
                {
                    Succeed = true,
                    Data = false
                };

            var headerToken = Request.Headers.GetValues("AuthToken").First();
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(headerToken);


            return new ApiResponse()
            {
                Succeed = true,
                Data = true
            };
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetCurrentUser()
        {
            using (var context = new AgmDataContext())
            {
                var username = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Username == username);
                return new ApiResponse(true)
                {
                    Data = new
                    {
                        user.Id,
                        user.Name,
                        user.Picture,
                        user.Username
                    }
                };
            }
        }
    }
}
