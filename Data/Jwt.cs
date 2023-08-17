using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TheEstate.Models.AuthModels;
using static TheEstate.Data.Constants;

namespace TheEstate.Data
{
    public class Jwt
    {
        public static string GenerateJwt(ClaimsIdentityModel i, IConfiguration config)
        {
            string jwtToken;

            try
            {
                var issuer = config["Jwt:Issuer"];
                var audience = config["Jwt:Audience"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JWTClaimTypes.Id, i.Id!),
                    new Claim(JWTClaimTypes.Username, i.Username!),
                    new Claim(JWTClaimTypes.Email, i.Email!),
                    new Claim(JWTClaimTypes.MobilePhone, i.MobileNo!),
                    //new Claim(JWTClaimTypes.ResidentCategory, i.ResidentCategory!),
                }),

                    Expires = DateTime.UtcNow.AddMonths(3),
                    Audience = audience,
                    Issuer = issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)

                };

                SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
                jwtToken = jwtTokenHandler.WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }

            return jwtToken;
        }


        public static ClaimsIdentityModel GetClaimsIdentity(ClaimsIdentity i)
        {
            ClaimsIdentityModel userIdentity = new();
            IList<Claim> claim = i.Claims.ToList();

            if (claim.Count > 0)
            {
                userIdentity.Id = claim[0].Value;
                userIdentity.Username = claim[1].Value;
                userIdentity.Email = claim[2].Value;
                userIdentity.MobileNo = claim[3].Value;
                userIdentity.ResidentCategory = claim[4].Value;
            }

            return userIdentity;
        }



        public static bool ValidateToken(HttpContext httpContext, string id)
        {
            bool data = false;
            try
            {
                string token = httpContext.Request.Headers[HeaderNames.Authorization].ToString().Split(' ')[1];
                ClaimsIdentityModel claims = GetClaimsIdentity((ClaimsIdentity?)httpContext.User.Identity!);
                JwtSecurityToken securityToken = new JwtSecurityToken(token);
                if (securityToken.ValidTo >= DateTime.UtcNow && claims.Id == id) data = true;
            }
            catch (Exception)
            {
                data = false;
            }
            return data;
        }


        public static ClaimsIdentityModel ClaimsCredentials(HttpContext httpContext)
        {
            ClaimsIdentity? identity = (ClaimsIdentity?)httpContext?.User.Identity;
            ClaimsIdentityModel tokenClaims = GetClaimsIdentity(identity!);
            return tokenClaims;
        }


        public class IdentityPolicy
        {
            public const string RoleClaimPolicy = "Role1";
            public const string RoleClaimName = "DELETE";

        }

    }
}
