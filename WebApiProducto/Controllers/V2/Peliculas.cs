using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiProducto.Data;
using AutoMapper;
using WebApiProducto.DTOs;
using WebApiProducto.Entidades;
using Microsoft.EntityFrameworkCore;

namespace WebApiProducto.Controllers.V2
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Peliculas : ControllerBase
    {
         private readonly ILogger<Generos> _logger;
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _imapper;

        public Peliculas(ILogger<Generos> logger, ApplicationDbContext context, IMapper imapper)
        {
            this._imapper = imapper;
            this._Context = context;
            this._logger = logger;
        }
        [HttpPost]
        [ProducesResponseType(typeof(PeliculaCreacionDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = _imapper.Map<Pelicula>(peliculaCreacionDTO);

            if (pelicula.PeliculasActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1;
                }
            }

            _Context.Add(pelicula);
            await _Context.SaveChangesAsync();
            return Ok(peliculaCreacionDTO);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pelicula>> Get(int id)
        {
            var pelicula = await _Context.Peliculas
                .Include(p => p.Comentarios)
                .Include(p => p.GenerosPeliculas)
                    .ThenInclude(pa => pa.Genero)
                .Include(p => p.PeliculasActores.OrderBy(pa => pa.Orden))
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }
         [HttpGet("select/{id:int}")]
        public async Task<ActionResult> GetSelect(int id)
        {
            var pelicula = await _Context.Peliculas
                .Select(pel => new
                {
                    pel.Id,
                    pel.Titulo,
                    Generos = pel.GenerosPeliculas.Select(pa =>
                    new {
                        pa.Genero.Nombre
                    }),
                    Actores = pel.PeliculasActores.OrderBy(pa => pa.Orden).Select(pa =>
                    new {
                        pa.Actor.Nombre,
                        pa.Personaje
                    }),
                    CantidadComentarios = pel.Comentarios.Count()
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }
    }
}