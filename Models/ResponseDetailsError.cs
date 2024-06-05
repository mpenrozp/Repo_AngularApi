using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProducto.Models
{
    public static class ErrorDescription
    {
        public static readonly string NoControlado = "Error no controlado";
        public static readonly string Validacion = "Error de validación";
        public static readonly string Controlado = "Error controlado";
    }
    public class ResponseDetailsError : ProblemDetails
    {
        public Dictionary<string, string[]>? errors { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}