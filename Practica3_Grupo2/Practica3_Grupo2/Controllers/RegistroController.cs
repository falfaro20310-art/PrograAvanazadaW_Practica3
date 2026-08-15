using Microsoft.AspNetCore.Mvc;
using Practica3_Grupo2.Models;

namespace Practica3_Grupo2.Controllers
{
    public class RegistroController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly string _apiBaseUrl;

        public RegistroController(IHttpClientFactory http, IConfiguration configuration)
        {
            _http = http;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]!;
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            using var client = _http.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}api/Compras/pendientes");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "No se pudo obtener la lista de compras pendientes.");
                return View(new RegistroViewModel());
            }

            var pendientes = await response.Content.ReadFromJsonAsync<List<CompraViewModel>>();

            var modelo = new RegistroViewModel
            {
                ComprasPendientes = pendientes ?? new List<CompraViewModel>()
            };

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            using var client = _http.CreateClient();
            var request = new
            {
                CodigoCompra = model.CodigoCompra,
                Monto = model.Abono
            };
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}api/Compras/abono", request);

            if (!response.IsSuccessStatusCode)
            {var mensajeError = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"No se pudo registrar el abono: {mensajeError}");

                var reintento = await client.GetAsync($"{_apiBaseUrl}api/Compras/pendientes");
                var pendientes = await reintento.Content.ReadFromJsonAsync<List<CompraViewModel>>();
                model.ComprasPendientes = pendientes ?? new List<CompraViewModel>();

                return View(model);
            }
            return RedirectToAction("Consulta", "Consulta");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSaldo(long codigoCompra)
        {
            using var client = _http.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}api/Compras/{codigoCompra}/saldo");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var saldo = await response.Content.ReadFromJsonAsync<decimal>();
            return Json(saldo);
        }
    }
}