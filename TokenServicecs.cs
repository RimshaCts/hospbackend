using HospitalAppointment.Auth;
using HospitalAppointment.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using webapi.Models;

namespace HospitalAppointment.Auth
{
    public class TokenService : ITokenService

    {

        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)

        {

            _config = config;

        }

        public string CreateToken(User us)

        {

            var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, us.UserId .ToString()),
    new Claim(ClaimTypes.Email, us.Email),
    new Claim(ClaimTypes.Role, us.Role)
};


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: _config["Jwt:Issuer"],

                audience: _config["Jwt:Audience"],

                claims: claims,

                expires: DateTime.Now.AddHours(2),

                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
