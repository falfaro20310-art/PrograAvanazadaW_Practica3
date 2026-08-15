using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Practica3_API.Data;
using Practica3_API.Models;

namespace Practica3_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly IComprasRepository _repositorio;

        public ComprasController(IComprasRepository repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> ListarTodos()
        {
            var compras = await _repositorio.ListarTodosAsync();
            return Ok(compras);
        }

        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<Compra>>> ListarPendientes()
        {
            var compras = await _repositorio.ListarPendientesAsync();
            return Ok(compras);
        }

        [HttpGet("{codigoCompra:long}/saldo")]
        public async Task<ActionResult<decimal>> ObtenerSaldo(long codigoCompra)
        {
            var saldo = await _repositorio.ObtenerSaldoAnteriorAsync(codigoCompra);
            if (saldo == null)
                return NotFound($"No existe la compra con codigo {codigoCompra}.");

            return Ok(saldo);
        }

        [HttpPost("abono")]
        public async Task<ActionResult<AbonoResult>> RegistrarAbono([FromBody] AbonoRequest request)
        {
            if (request.Monto <= 0)
                return BadRequest("El monto del abono debe ser mayor a cero.");

            try
            {
                var resultado = await _repositorio.RegistrarAbonoAsync(request.CodigoCompra, request.Monto);
                return Ok(resultado);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
