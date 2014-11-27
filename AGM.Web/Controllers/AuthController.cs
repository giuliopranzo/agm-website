using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace AGM.Web.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        public ApiResponse Login(dynamic loginData)
        {
            var now = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, loginData.Email.ToString()) 
            };

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = "Agm",
                Lifetime = new Lifetime(now, now.AddMinutes(30))
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

            return new ApiResponse() {};
        }
    }
}
