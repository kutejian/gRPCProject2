using gRPC.Framework;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gRPC.AuthCenter.Utility
{
    public class JWTHSService : IJWTService
    {
        #region Option注入
        private readonly JWTTokenOptions _JWTTokenOptions;
        public JWTHSService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
        {
            _JWTTokenOptions = jwtTokenOptions.CurrentValue;
        }
        #endregion

        public string GetToken(CurrentUserModel userInfo)
        {
            var claims = new[]
            {
                   new Claim(ClaimTypes.Name, userInfo.Name),
                   new Claim("EMail", userInfo.EMail),
                   new Claim("Account", userInfo.Account),
                   new Claim("Age", userInfo.Age.ToString()),
                   new Claim("Id", userInfo.Id.ToString()),
                   new Claim("Mobile", userInfo.Mobile),
                   new Claim(ClaimTypes.Role,userInfo.Role),
                   //new Claim("Role", userInfo.Role),//这个不能角色授权
                   new Claim("Sex", userInfo.Sex.ToString())//各种信息拼装
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTTokenOptions.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _JWTTokenOptions.Issuer,
                audience: _JWTTokenOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),//5分钟有效期
                notBefore: DateTime.Now.AddMinutes(1),//1分钟后有效
                signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}
