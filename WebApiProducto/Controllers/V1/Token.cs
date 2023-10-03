using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using WebApiProducto.Interfaces;
using WebApiProducto.Models;

namespace WebApiProducto.Controllers.V1
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]
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
        [ProducesResponseType(typeof(AutorizacionRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetails), StatusCodes.Status500InternalServerError)]
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

    }
}