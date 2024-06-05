using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApiProducto.Data;
using AutoMapper;
using WebApiProducto.DTOs;
using WebApiProducto.Entidades;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace WebApiProducto.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Actores : ControllerBase
    {
        private readonly ILogger<Generos> _logger;
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _imapper;

        public Actores(ILogger<Generos> logger, ApplicationDbContext context, IMapper imapper)
        {
            this._imapper = imapper;
            this._Context = context;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get()
        {
            return await _Context.Actores.OrderByDescending(a => a.FechaNacimiento).ToListAsync();
        }
        [HttpGet("nombre")]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Get(string nombre)
        {
            // Versión 1
            return await _Context.Actores
                .Where(a => a.Nombre == nombre)
                .OrderBy(a => a.Nombre)
                .ThenByDescending(a => a.FechaNacimiento)
                .ProjectTo<ActorDTO>(_imapper.ConfigurationProvider)
                .ToListAsync();
        }
        [HttpGet("nombre/v2")]
        [ProducesResponseType(typeof(IEnumerable<ActorDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetV2(string nombre)
        {
            // Versión 2: Contiene
            return await _Context.Actores
                .Where(a => a.Nombre.Contains(nombre))
                .ProjectTo<ActorDTO>(_imapper.ConfigurationProvider)
                .ToListAsync();
        }
        [HttpGet("fechaNacimiento/rango")]
        [ProducesResponseType(typeof(IEnumerable<ActorDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Get(DateTime inicio,
            DateTime fin)
        {
            return await _Context.Actores
                .Where(a => a.FechaNacimiento >= inicio && a.FechaNacimiento <= fin)
                .ProjectTo<ActorDTO>(_imapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ActorDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await _Context.Actores.FirstOrDefaultAsync(a => a.Id == id);
            var actorDTO = _imapper.Map<ActorDTO>(actor);
            if (actor is null)
            {
                return NotFound();
            }

            return actorDTO;
        }
        [HttpGet("idynombre")]
        [ProducesResponseType(typeof(IEnumerable<ActorDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Getidynombre()
        {
            return await _Context.Actores
                .ProjectTo<ActorDTO>(_imapper.ConfigurationProvider)
                .ToListAsync();
        }
        [HttpPost]
        [ProducesResponseType(typeof(ActorCreacionDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult> Post(ActorCreacionDTO actorCreacionDTO)
        {
            var actor = _imapper.Map<Actor>(actorCreacionDTO);

            _Context.Add(actor);
            await _Context.SaveChangesAsync();
            return Ok(actorCreacionDTO);
        }

    }
}