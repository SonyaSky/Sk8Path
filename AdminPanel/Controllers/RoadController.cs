using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminPanel.Controllers
{
    public class RoadController : Controller
    {
        private readonly HttpClient _client;

        public RoadController(HttpClient httpClient)
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
                var response = await _client.GetAsync("http://95.182.120.75:8082/api/road/deleting");
                if (!response.IsSuccessStatusCode) return View("Error");

                var json = await response.Content.ReadAsStringAsync();
                var roads = JsonConvert.DeserializeObject<List<RoadDto>>(json);
                return View(roads);
            }
            catch (Exception)
            {
                ViewBag.Error = "Сервис недоступен.";
                return View(new List<RoadDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                return RedirectToAction("Login", "Account");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.DeleteAsync($"http://95.182.120.75:8082/api/road/{id}/approve");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Ошибка при удалении дороги.";
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
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://95.182.120.75:8082/api/road/{id}/decline");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Ошибка при отклонении дороги.";
            }

            return RedirectToAction("Index");
        }
    }
}