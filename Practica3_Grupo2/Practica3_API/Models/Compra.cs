namespace Practica3_API.Models
{
    public class Compra
    {
        public long CodigoCompra { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
