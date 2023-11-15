using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiProducto.Entidades
{
    public class GeneroPelicula
    {
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; } = null!;
        public int GeneroId { get; set; }
        public Genero Genero { get; set; } = null!;
    }
}