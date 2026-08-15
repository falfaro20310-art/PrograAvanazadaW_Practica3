namespace Practica3_Grupo2.Models
{
    public class CompraViewModel
    {
        public long CodigoCompra { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
