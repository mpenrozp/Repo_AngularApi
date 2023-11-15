using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public HashSet<Comentario> Comentarios { get; set; } = new HashSet<Comentario>();
        public List<GeneroPelicula> GenerosPeliculas { get; set; } = new List<GeneroPelicula>();
        public List<PeliculaActor> PeliculasActores { get; set; } = new List<PeliculaActor>();

    }
}