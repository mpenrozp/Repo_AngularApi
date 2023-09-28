using System.Text.Json;

namespace WebApiProducto.Models
{
    public static class ErrorDescription
    {
        public static readonly string NoControlado = "Error no controlado";
        public static readonly string Validacion = "Error de validación";
        public static readonly string Controlado = "Error controlado";
    }
    public class ResponseDetails
    {
        public int status { get; set; }
        public string title { get; set; } = string.Empty;
         public string details { get; set; } = string.Empty;
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}