namespace mercharteria.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
