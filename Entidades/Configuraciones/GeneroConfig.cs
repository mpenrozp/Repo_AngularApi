using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApiProducto.Entidades.Configuraciones
{
    public class GeneroConfig : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            var cienciaFiccion = new Genero { Id = 5, Nombre = "Ciencia Ficción" };
            var animacion = new Genero { Id = 6, Nombre = "Animación" };
            builder.HasData(cienciaFiccion, animacion);

            builder.HasIndex(p => p.Nombre).IsUnique();
        }
    }
}