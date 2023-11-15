using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.DTOs
{
    public class ComentarioCreacionDTO
    {
        public int PeliculaId { get; set; }
        public string? Contenido { get; set; }
        public bool Recomendar { get; set; }
    }
}