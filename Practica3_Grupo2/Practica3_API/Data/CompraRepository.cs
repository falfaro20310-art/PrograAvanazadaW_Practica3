using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Practica3_API.Models;

namespace Practica3_API.Data
{
    public interface IComprasRepository
    {
        Task<IEnumerable<Compra>> ListarTodosAsync();
        Task<IEnumerable<Compra>> ListarPendientesAsync();
        Task<decimal?> ObtenerSaldoAnteriorAsync(long codigoCompra);
        Task<AbonoResult> RegistrarAbonoAsync(long codigoCompra, decimal monto);
    }

    public class ComprasRepository : IComprasRepository
    {
        private readonly string _connectionString;

        public ComprasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontro la cadena de conexion 'DefaultConnection' en appsettings.json.");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Compra>> ListarTodosAsync()
        {
            using var conn = CreateConnection();
            return await conn.QueryAsync<Compra>(
                "sp_ListarProductos",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Compra>> ListarPendientesAsync()
        {
            using var conn = CreateConnection();
            return await conn.QueryAsync<Compra>(
                "sp_ListarPendientes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<decimal?> ObtenerSaldoAnteriorAsync(long codigoCompra)
        {
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<decimal?>(
                "sp_ObtenerSaldoAnterior",
                new { CodigoCompra = codigoCompra },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<AbonoResult> RegistrarAbonoAsync(long codigoCompra, decimal monto)
        {
            using var conn = CreateConnection();
            var resultado = await conn.QueryFirstAsync<AbonoResult>(
                "sp_RegistrarAbono",
                new { CodigoCompra = codigoCompra, Monto = monto },
                commandType: CommandType.StoredProcedure);
            return resultado;
        }
    }
}
