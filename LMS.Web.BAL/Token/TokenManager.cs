using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Token
{
    public class TokenManager
    {
        private static string Key =ConfigurationManager.AppSettings.Get("jwtkey") ;
        public static string GenerateToken(string email)
        {

            byte[] key = Encoding.BigEndianUnicode.GetBytes(Key);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                //if (jwtSecurityToken == null)
                //{
                //    return null;
                //}
                byte[] key = Encoding.BigEndianUnicode.GetBytes(Key);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }


        }


        /// <summary>
        /// Validates the token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Email which was used to generate token</returns>
        public static string ValidateToken(string token)
        {
            string email = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
            {
                return "null";
            }
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return "Exception";
            }
            Claim emailClaim = identity.FindFirst(ClaimTypes.Email);
            email = emailClaim.Value;
            return email;
        }
    }
}
