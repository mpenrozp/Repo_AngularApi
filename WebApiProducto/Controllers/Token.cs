using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using WebApiProducto.Interfaces;
using WebApiProducto.Models;

namespace WebApiProducto.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Token : ControllerBase
    {
        private readonly IToken _itoken;
        private readonly ILogger<Token> _logger;
        private readonly IConfiguration _configuration;
        public Token(IToken itoken, ILogger<Token> logger, IConfiguration configuration)
        {

            this._itoken = itoken;
            this._logger = logger;
            this._configuration = configuration;
        }

        [HttpPost("Login")]
        [MapToApiVersion("1.0")]
        public IResult Login(AutorizacionRequest autorizacion)
        {
            //Thread.Sleep(20000);
            //logger.LogInformation("{@autorizacion}", autorizacion);
            var validator = new UserValidator();
            var result = validator.Validate(autorizacion);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary(), null, null, null, ErrorDescription.Validacion);
            }
            var tokenresp = _itoken.GenerateToken(autorizacion.UserName, autorizacion.Password);

            AutorizacionRequest autorizacionRequest = new()
            {
                UserName = autorizacion.UserName,
                Password = autorizacion.Password,
                Token = tokenresp
            };

            return Results.Ok(autorizacionRequest);
        }
        [HttpPost("Login")]
        [MapToApiVersion("2.0")]
        public IResult Login_v2(AutorizacionRequest autorizacion)
        {
            throw new HttpRequestException("Método no implementado en esta versión", new Exception(), HttpStatusCode.NotImplemented);
        }

    }
}