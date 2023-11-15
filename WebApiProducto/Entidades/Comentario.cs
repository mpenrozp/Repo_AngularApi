using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string? Contenido { get; set; }
        public bool Recomendar { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; } = null!;
    }
}