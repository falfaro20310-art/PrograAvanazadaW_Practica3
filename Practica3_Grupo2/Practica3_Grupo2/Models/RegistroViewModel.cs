namespace Practica3_Grupo2.Models
{
    public class RegistroViewModel
    {
        public List<CompraViewModel> ComprasPendientes { get; set; } = new();
        public long CodigoCompra { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal Abono { get; set; }
    }
}