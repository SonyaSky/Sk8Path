using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://95.182.120.75:8082/api/auth/login", model);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                    return View(model);
                }

                var authResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
                {
                    ModelState.AddModelError(string.Empty, "Сервер авторизации вернул пустой ответ.");
                    return View(model);
                }

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(authResponse.AccessToken);

                var roles = token.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

                if (!roles.Contains("Admin"))
                {
                    ModelState.AddModelError(string.Empty, "У вас нет прав доступа (требуется роль Admin)");
                    return View(model);
                }

                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                });

                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(2)
                });

                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Ошибка соединения с сервером авторизации.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла непредвиденная ошибка.");
            }

            return View(model);

        }
    }
}