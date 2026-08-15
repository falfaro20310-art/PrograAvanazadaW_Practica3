namespace Practica3_API.Models
{
    public class AbonoRequest
    {
        public long CodigoCompra { get; set; }
        public decimal Monto { get; set; }
    }

    public class AbonoResult
    {
        public decimal Saldo { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
