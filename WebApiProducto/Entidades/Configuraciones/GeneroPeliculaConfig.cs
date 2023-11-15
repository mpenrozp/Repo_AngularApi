using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApiProducto.Entidades.Configuraciones
{
    public class GeneroPeliculaConfig : IEntityTypeConfiguration<GeneroPelicula>
    {
        public void Configure(EntityTypeBuilder<GeneroPelicula> builder)
        {
            builder.HasKey(pa => new { pa.GeneroId, pa.PeliculaId });
        }
    }
}