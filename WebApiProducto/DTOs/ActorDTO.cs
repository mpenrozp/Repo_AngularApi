using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Fortuna { get; set; }
    }
}