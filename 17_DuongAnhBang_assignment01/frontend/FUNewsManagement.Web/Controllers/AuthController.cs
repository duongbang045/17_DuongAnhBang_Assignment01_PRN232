using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Web.Helpers;
using FUNewsManagement.Web.Models;
using FUNewsManagement.Web.Services;

namespace FUNewsManagement.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiClient _apiClient;

        public AuthController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetUserSession() != null)
            {
                return RedirectToRoleHome();
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var endpoint = "api/auth/login";
                var payload = new { Email = model.Email, Password = model.Password };
                var result = await _apiClient.PostAsync<UserSession>(endpoint, payload);
                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }
                HttpContext.Session.SetUserSession(result);
                return RedirectToRoleHome();
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.ClearUserSession();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToRoleHome()
        {
            var user = HttpContext.Session.GetUserSession();
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.Role == "Admin")
            {
                return RedirectToAction("Accounts", "Admin");
            }
            return RedirectToAction("Categories", "Staff");
        }
    }
}
