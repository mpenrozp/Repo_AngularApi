using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiProducto.Interfaces;
using WebApiProducto.Models;

namespace WebApiProducto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Token : ControllerBase
    {
        private readonly IToken itoken;
        private readonly ILogger<Token> logger;
        private readonly IConfiguration configuration;
        public Token(IToken _itoken, ILogger<Token> _logger, IConfiguration _configuration)
        {

            this.itoken = _itoken;
            this.logger = _logger;
            this.configuration = _configuration;
        }
        [HttpPost()]
        public IResult Login(AutorizacionRequest autorizacion)
        {
            var validator = new UserValidator();

            var result = validator.Validate(autorizacion);
            if (!result.IsValid)
            {
                 return Results.ValidationProblem(result.ToDictionary());
            }
            var tokenresp = itoken.GenerateToken(autorizacion.UserName, autorizacion.Password);

            AutorizacionRequest autorizacionRequest = new()
            {
                UserName = autorizacion.UserName,
                Password = autorizacion.Password,
                Token = tokenresp
            };

            return Results.Ok(autorizacionRequest);
        }
    }
}