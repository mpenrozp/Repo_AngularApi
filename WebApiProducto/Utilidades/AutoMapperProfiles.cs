using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApiProducto.DTOs;
using WebApiProducto.Entidades;

namespace WebApiProducto.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<ActorCreacionDTO, Actor>();
            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<PeliculaCreacionDTO, Pelicula>();
   //             .ForMember(ent => ent.Generos, dto =>
   //           dto.MapFrom(campo => campo.Generos.Select(id => new Genero { Id = id })));
            CreateMap<PeliculaActorCreacionDTO, PeliculaActor>();
            CreateMap<GeneroPeliculaCreacionDTO, GeneroPelicula>();

            CreateMap<Actor, ActorDTO>();
        }
    }
}