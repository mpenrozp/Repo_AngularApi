using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.DTOs
{
    public class GeneroCreacionDTO
    {
        [StringLength(maximumLength: 150)]
        public string Nombre { get; set; } = null!;
    }
}