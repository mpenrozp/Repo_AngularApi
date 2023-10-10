using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using WebApiProducto.Models;

namespace WebApiProducto.Examples
{
    public class AutorizacionRequestExample : IMultipleExamplesProvider<AutorizacionRequest>
    {
        public IEnumerable<SwaggerExample<AutorizacionRequest>> GetExamples()
        {

            yield return SwaggerExample.Create("Example 1",
            new AutorizacionRequest()
            {
                UserName = "admin",
                Password = "admin",

            });

        }
    }
}