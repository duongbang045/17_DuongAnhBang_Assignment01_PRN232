using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Web.Helpers;
using FUNewsManagement.Web.Models;
using FUNewsManagement.Web.Services;

namespace FUNewsManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IApiClient _apiClient;

        public AdminController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        private string GetToken() => HttpContext.Session.GetUserSession()?.Token;
        private UserSession GetUser() => HttpContext.Session.GetUserSession();
        private bool IsAdmin() => GetUser()?.Role == "Admin";
        private IActionResult RequireAdmin() => !IsAdmin() ? RedirectToAction("Login", "Auth") : null;

        public async Task<IActionResult> Accounts(string keyword = null)
        {
            var auth = RequireAdmin();
            if (auth != null) return auth;
            try
            {
                string endpoint = string.IsNullOrEmpty(keyword) ? "api/accounts" : $"api/accounts/search?keyword={Uri.EscapeDataString(keyword)}";
                var list = await _apiClient.GetAsync<IEnumerable<SystemAccount>>(endpoint, GetToken());
                ViewBag.Keyword = keyword;
                return View(list ?? new List<SystemAccount>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<SystemAccount>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(SystemAccount model)
        {
            var auth = RequireAdmin();
            if (auth != null) return auth;
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Accounts));
            }
            if (string.IsNullOrEmpty(model.AccountPassword)) model.AccountPassword = "@1";
            try
            {
                await _apiClient.PostRawAsync("api/accounts", model, GetToken());
                return RedirectToAction(nameof(Accounts));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Accounts));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(SystemAccount model)
        {
            var auth = RequireAdmin();
            if (auth != null) return auth;
            try
            {
                await _apiClient.PutRawAsync($"api/accounts/{model.AccountID}", model, GetToken());
                return RedirectToAction(nameof(Accounts));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Accounts));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            var auth = RequireAdmin();
            if (auth != null) return auth;
            try
            {
                await _apiClient.DeleteRawAsync($"api/accounts/{id}", GetToken());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Accounts));
        }

        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            var auth = RequireAdmin();
            if (auth != null) return auth;
            var start = startDate ?? new DateTime(DateTime.Today.Year, 1, 1);
            var end = endDate ?? DateTime.Today;
            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");
            try
            {
                var endpoint = $"api/newsarticles/report?startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}";
                var report = await _apiClient.GetAsync<IEnumerable<ReportStatistic>>(endpoint, GetToken());
                return View(report ?? new List<ReportStatistic>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<ReportStatistic>());
            }
        }
    }
}
