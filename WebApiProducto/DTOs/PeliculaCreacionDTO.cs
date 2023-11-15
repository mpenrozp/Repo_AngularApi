using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProducto.Entidades;

namespace WebApiProducto.DTOs
{
    public class PeliculaCreacionDTO
    {
        public string Titulo { get; set; } = null!;
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public List<GeneroPeliculaCreacionDTO> GenerosPeliculas { get; set; } = new List<GeneroPeliculaCreacionDTO>();
         public List<PeliculaActorCreacionDTO> PeliculasActores { get; set; } 
                = new List<PeliculaActorCreacionDTO>();
    }
}