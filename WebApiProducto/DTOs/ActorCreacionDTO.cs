using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.DTOs
{
    public class ActorCreacionDTO
    {
        [StringLength(150)]
        public string Nombre { get; set; } = null!;
        public decimal Fortuna { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}