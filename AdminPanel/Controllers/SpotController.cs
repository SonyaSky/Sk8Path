using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminPanel.Controllers
{
    public class SpotController : Controller
    {
        private readonly HttpClient _client;

        public SpotController(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var response = await _client.GetAsync("http://95.182.120.75:8082/api/spot/deleting");
                if (!response.IsSuccessStatusCode) return View("Error");

                var json = await response.Content.ReadAsStringAsync();
                var spots = JsonConvert.DeserializeObject<List<SpotDto>>(json);
                return View(spots);
            }
            catch (Exception)
            {
                ViewBag.Error = "Сервис недоступен.";
                return View(new List<SpotDto>());
            }
        }
    }
}