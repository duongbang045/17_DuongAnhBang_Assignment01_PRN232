using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Web.Models;
using FUNewsManagement.Web.Services;

namespace FUNewsManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiClient _apiClient;

        public HomeController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(string keyword = null)
        {
            try
            {
                string endpoint = "api/newsarticles/active";
                if (!string.IsNullOrEmpty(keyword))
                {
                    var all = await _apiClient.GetAsync<IEnumerable<NewsArticleDetailViewModel>>(endpoint);
                    all = all.Where(n =>
                        (n.NewsTitle != null && n.NewsTitle.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (n.Headline != null && n.Headline.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (n.NewsContent != null && n.NewsContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    );
                    ViewBag.Keyword = keyword;
                    return View(all);
                }
                var list = await _apiClient.GetAsync<IEnumerable<NewsArticleDetailViewModel>>(endpoint);
                return View(list);
            }
            catch
            {
                return View(new List<NewsArticleDetailViewModel>());
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            try
            {
                var article = await _apiClient.GetAsync<NewsArticleDetailViewModel>($"api/newsarticles/active/{id}");
                if (article == null) return NotFound();
                return View(article);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
