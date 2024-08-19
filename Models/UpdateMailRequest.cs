namespace WebApiProducto.Models
{
    public class UpdateMailRequest
    {
        public int Identificador { get; set; }
        public string Mail { get; set; } = string.Empty;

    }
}
