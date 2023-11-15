using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using WebApiProducto.Data;
using WebApiProducto.Entidades;
using WebApiProducto.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebApiProducto.Controllers.V2
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Generos : ControllerBase
    {
        private readonly ILogger<Generos> _logger;
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _imapper;

        public Generos(ILogger<Generos> logger, ApplicationDbContext context, IMapper imapper)
        {
            this._imapper = imapper;
            this._Context = context;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await _Context.Generos.ToListAsync();
        }
        [HttpPost]
        [ProducesResponseType(typeof(GeneroCreacionDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult> Post(GeneroCreacionDTO generoCreacionDTO)
        {
            var genero1 = _imapper.Map<Genero>(generoCreacionDTO);

            _Context.Add(genero1);
            await _Context.SaveChangesAsync();
            return Ok(generoCreacionDTO);
        }
        [HttpPost("varios")]
        [ProducesResponseType(typeof(GeneroCreacionDTO[]), StatusCodes.Status200OK)]
        public async Task<ActionResult> Post(GeneroCreacionDTO[] generoCreacionDTO)
        {
            var genero1 = _imapper.Map<Genero[]>(generoCreacionDTO);

            _Context.AddRange(genero1);
            await _Context.SaveChangesAsync();
            return Ok(generoCreacionDTO);
        }
         [HttpPut("{id:int}/nombre2")]
        public async Task<ActionResult> Put(int id)
        {
            var genero = await _Context.Generos.FirstOrDefaultAsync(g => g.Id == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.Nombre = genero.Nombre + "2";

            await _Context.SaveChangesAsync();
            return Ok();

        }
         [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _imapper.Map<Genero>(generoCreacionDTO);
            genero.Id = id;
            _Context.Update(genero);
            await _Context.SaveChangesAsync();
            return Ok();
        }

    }
}