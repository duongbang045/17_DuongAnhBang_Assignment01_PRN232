using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Web.Helpers;
using FUNewsManagement.Web.Models;
using FUNewsManagement.Web.Services;

namespace FUNewsManagement.Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly IApiClient _apiClient;

        public StaffController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        private string GetToken() => HttpContext.Session.GetUserSession()?.Token;
        private UserSession GetUser() => HttpContext.Session.GetUserSession();
        private bool IsStaff() => GetUser() != null && (GetUser().Role == "Staff" || GetUser().Role == "Lecturer");
        private IActionResult RequireStaff() => !IsStaff() ? RedirectToAction("Login", "Auth") : null;

        // ========== CATEGORIES ==========
        public async Task<IActionResult> Categories(string keyword = null)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                string endpoint = string.IsNullOrEmpty(keyword) ? "api/categories" : $"api/categories/search?keyword={Uri.EscapeDataString(keyword)}";
                var list = await _apiClient.GetAsync<IEnumerable<Category>>(endpoint, GetToken());
                ViewBag.Keyword = keyword;
                return View(list ?? new List<Category>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<Category>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            if (!ModelState.IsValid) return RedirectToAction(nameof(Categories));
            try
            {
                await _apiClient.PostRawAsync("api/categories", model, GetToken());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(Category model)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                await _apiClient.PutRawAsync($"api/categories/{model.CategoryID}", model, GetToken());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                await _apiClient.DeleteRawAsync($"api/categories/{id}", GetToken());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Categories));
        }

        // ========== NEWS ARTICLES ==========
        public async Task<IActionResult> NewsArticles(string keyword = null)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                string endpoint = string.IsNullOrEmpty(keyword) ? "api/newsarticles" : $"api/newsarticles/search?keyword={Uri.EscapeDataString(keyword)}";
                var list = await _apiClient.GetAsync<IEnumerable<NewsArticleDetailViewModel>>(endpoint, GetToken());
                ViewBag.Keyword = keyword;
                return View(list ?? new List<NewsArticleDetailViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<NewsArticleDetailViewModel>());
            }
        }

        public async Task<IActionResult> CreateNews()
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            var model = new NewsArticleCreateViewModel
            {
                NewsArticleID = DateTime.Now.Ticks.ToString(),
                CreatedDate = DateTime.Now,
                NewsStatus = true
            };
            try
            {
                model.Categories = await _apiClient.GetAsync<IEnumerable<Category>>("api/categories", GetToken());
                model.Tags = await _apiClient.GetAsync<IEnumerable<Tag>>("api/tags", GetToken());
            }
            catch { }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNews(NewsArticleCreateViewModel model)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            if (!ModelState.IsValid)
            {
                try
                {
                    model.Categories = await _apiClient.GetAsync<IEnumerable<Category>>("api/categories", GetToken());
                    model.Tags = await _apiClient.GetAsync<IEnumerable<Tag>>("api/tags", GetToken());
                }
                catch { }
                return View(model);
            }
            try
            {
                var user = GetUser();
                var payload = new
                {
                    model.NewsArticleID,
                    model.NewsTitle,
                    model.Headline,
                    model.CreatedDate,
                    model.NewsContent,
                    model.NewsSource,
                    model.CategoryID,
                    model.NewsStatus,
                    CreatedByID = user.AccountID,
                    TagIds = model.TagIds ?? new List<int>()
                };
                await _apiClient.PostRawAsync("api/newsarticles", payload, GetToken());
                return RedirectToAction(nameof(NewsArticles));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                try
                {
                    model.Categories = await _apiClient.GetAsync<IEnumerable<Category>>("api/categories", GetToken());
                    model.Tags = await _apiClient.GetAsync<IEnumerable<Tag>>("api/tags", GetToken());
                }
                catch { }
                return View(model);
            }
        }

        public async Task<IActionResult> UpdateNews(string id)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                var detail = await _apiClient.GetAsync<NewsArticleDetailViewModel>($"api/newsarticles/{id}", GetToken());
                if (detail == null) return NotFound();
                var model = new NewsArticleUpdateViewModel
                {
                    NewsArticleID = detail.NewsArticleID,
                    NewsTitle = detail.NewsTitle,
                    Headline = detail.Headline,
                    NewsContent = detail.NewsContent,
                    NewsSource = detail.NewsSource,
                    CategoryID = detail.CategoryID,
                    NewsStatus = detail.NewsStatus,
                    TagIds = detail.Tags?.Select(t => t.TagID).ToList() ?? new List<int>()
                };
                model.Categories = await _apiClient.GetAsync<IEnumerable<Category>>("api/categories", GetToken());
                model.Tags = await _apiClient.GetAsync<IEnumerable<Tag>>("api/tags", GetToken());
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(NewsArticles));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNews(NewsArticleUpdateViewModel model)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            if (!ModelState.IsValid)
            {
                try
                {
                    model.Categories = await _apiClient.GetAsync<IEnumerable<Category>>("api/categories", GetToken());
                    model.Tags = await _apiClient.GetAsync<IEnumerable<Tag>>("api/tags", GetToken());
                }
                catch { }
                return View(model);
            }
            try
            {
                var user = GetUser();
                var payload = new
                {
                    model.NewsTitle,
                    model.Headline,
                    model.NewsContent,
                    model.NewsSource,
                    model.CategoryID,
                    model.NewsStatus,
                    UpdatedByID = user.AccountID,
                    ModifiedDate = DateTime.Now,
                    TagIds = model.TagIds ?? new List<int>()
                };
                await _apiClient.PutRawAsync($"api/newsarticles/{model.NewsArticleID}", payload, GetToken());
                return RedirectToAction(nameof(NewsArticles));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                try
                {
                    model.Categories = await _apiClient.GetAsync<IEnumerable<Category>>("api/categories", GetToken());
                    model.Tags = await _apiClient.GetAsync<IEnumerable<Tag>>("api/tags", GetToken());
                }
                catch { }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                await _apiClient.DeleteRawAsync($"api/newsarticles/{id}", GetToken());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(NewsArticles));
        }

        // ========== PROFILE & HISTORY ==========
        public async Task<IActionResult> Profile()
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                var account = await _apiClient.GetAsync<SystemAccount>("api/profile", GetToken());
                var model = new UpdateProfileViewModel
                {
                    AccountName = account.AccountName,
                    AccountEmail = account.AccountEmail,
                    AccountPassword = ""
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new UpdateProfileViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UpdateProfileViewModel model)
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            if (!ModelState.IsValid) return View(model);
            try
            {
                await _apiClient.PutRawAsync("api/profile", model, GetToken());
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> NewsHistory()
        {
            var auth = RequireStaff();
            if (auth != null) return auth;
            try
            {
                var list = await _apiClient.GetAsync<IEnumerable<NewsArticleDetailViewModel>>("api/profile/news-history", GetToken());
                return View(list ?? new List<NewsArticleDetailViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<NewsArticleDetailViewModel>());
            }
        }
    }
}
