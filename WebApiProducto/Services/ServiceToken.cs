using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiProducto.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace WebApiProducto.Services
{
    public class ServiceToken : IToken
    {
        private readonly ILogger<ServiceToken> logger;
        private readonly IConfiguration configuration;
        public ServiceToken(ILogger<ServiceToken> _logger, IConfiguration _configuration)
        {

            this.logger =_logger;
            this.configuration = _configuration;
        }
        public string GenerateToken(string UserName, string Password)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Ke"]!));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            
            var claims = new []{
                new Claim(ClaimTypes.NameIdentifier, UserName)
            };
            var token = new JwtSecurityToken(
                        configuration["Jwt:Issuer"],
                        configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}