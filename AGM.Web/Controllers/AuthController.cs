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
            string email = loginData.Email;
            string password = loginData.Password;
            string name = string.Empty;

            using (var context = new AgmDataContext())
            {
                if (context.Users.All(u => u.Email.ToLower() != email.ToLower() || u.Password != password || u._sectionMonthlyReportsVisible != 1 || u._isDeleted || u._isActive != 1 ))
                    return new ApiResponse(false)
                    {
                        Errors =
                            new ApiResponseError[]
                            {new ApiResponseError() {Message = "Email o password errati"}}
                    };

                name = context.Users.First(u => u.Email.ToLower() == email.ToLower() && u.Password == password).Name;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, string.Format("{0}${1}", loginData.Email.ToString(), name))
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
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email && !u._isDeleted && u._isActive == 1);
                return new ApiResponse(true)
                {
                    Data = new
                    {
                        user.Id,
                        user.Name,
                        user.Image,
                        user.Email,
                        user.SectionUsersVisible,
                        user.SectionJobAdsVisible
                    }
                };
            }
        }
    }
}
