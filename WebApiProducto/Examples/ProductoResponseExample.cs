using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using WebApiProducto.Models;

namespace WebApiProducto.Examples
{
    public class ProductoResponseExample : IMultipleExamplesProvider<List<Producto>>
    {
        public IEnumerable<SwaggerExample<List<Producto>>> GetExamples()
        {

            yield return SwaggerExample.Create("Example 1",
           new List<Producto>
            {
            new  Producto()
            {
                Id = 1,
                Price = 2000,
                Title = "Producto ejemplo",
                Images = new string[2] { "producto 1", "producto 2" }
            },
             new Producto()
            {
                Id = 2,
                Price = 3000,
                Title = "Producto ejemplo 2",
                Images = Array.Empty<string>()
            }
            });

        }
    }
    public class ProductoResponseErrorExample : IMultipleExamplesProvider<ResponseDetailsError>
    {

        public IEnumerable<SwaggerExample<ResponseDetailsError>> GetExamples()
        {
            Dictionary<string, string[]> errores = new Dictionary<string, string[]>();
            string[] mensajes = { "El usuario no puede ser vacío" };
            errores.Add("UserName", mensajes);

            yield return SwaggerExample.Create("Example Error 1",
             new ResponseDetailsError()
             {
                 Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.1",
                 Status = StatusCodes.Status500InternalServerError,
                 Detail = "Detalle del error",
                 Title = ErrorDescription.NoControlado,

             });
            yield return SwaggerExample.Create("Example Error 2",
            new ResponseDetailsError()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.1",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Parametros del request inválidos",
                Title = ErrorDescription.Validacion,
                errors = errores

            });


        }
    }
}