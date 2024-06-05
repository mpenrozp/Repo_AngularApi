using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.DTOs
{
    public class PeliculaActorCreacionDTO
    {
        public int ActorId { get; set; }
        public string Personaje { get; set; } = null!;
    }
}