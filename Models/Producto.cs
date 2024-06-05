namespace WebApiProducto.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public string[] Images { get; set; } = new string[100];

    }
}