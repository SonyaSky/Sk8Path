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

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.DeleteAsync($"http://95.182.120.75:8082/api/spot/{id}/approve");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Ошибка при удалении места.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id)
        {
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://95.182.120.75:8082/api/spot/{id}/decline");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Ошибка при отклонении места.";
            }

            return RedirectToAction("Index");
        }
    }
}