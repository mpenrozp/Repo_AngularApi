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

namespace WebApiProducto.Controllers.V2
{
    [ApiController]
    [Produces("application/json")]
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
        [MapToApiVersion("2.0")]
        public IResult Login(AutorizacionRequest autorizacion)
        {
            throw new HttpRequestException("Método no implementado en esta versión", new Exception(), HttpStatusCode.NotImplemented);
        }

    }
}