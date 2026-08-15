using Microsoft.AspNetCore.Mvc;
using Practica3_Grupo2.Models;

namespace Practica3_Grupo2.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly string _apiBaseUrl;

        public ConsultaController(IHttpClientFactory http, IConfiguration configuration)
        {
            _http = http;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]!;
        }

        public async Task<IActionResult> Consulta()
        {
            using var client = _http.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}api/Compras");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "No se pudo obtener la información de las compras.");
                return View();
            }

            var compras = await response.Content.ReadFromJsonAsync<List<CompraViewModel>>();
            return View(compras);
        }
    }
}
