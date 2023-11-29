using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApiProducto.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        //[StringLength(maximumLength:150)]
        public string Nombre { get; set; } = null!;
        
        public List<GeneroPelicula> GeneroPeliculas { get; set; } = new List<GeneroPelicula>();
    
    }
}