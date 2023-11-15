using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiProducto.Entidades;
using WebApiProducto.Entidades.Configuraciones;
using WebApiProducto.Entidades.Seeding;

namespace WebApiProducto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           /* modelBuilder.ApplyConfiguration(new ActorConfig());
            modelBuilder.ApplyConfiguration(new ComentarioConfig());
            modelBuilder.ApplyConfiguration(new PeliculaConfig());
            modelBuilder.ApplyConfiguration(new PeliculaActorConfig());*/
            SeedingInicial.Seed(modelBuilder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(150);
        }
        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<Comentario> Comentarios => Set<Comentario>();
        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
    }
}