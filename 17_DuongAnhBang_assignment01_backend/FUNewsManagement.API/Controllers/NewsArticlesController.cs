using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using FUNewsManagement.Models.DTOs;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [HttpGet("active")]
        [EnableQuery]
        [AllowAnonymous]
        public IActionResult GetActiveNews()
        {
            return Ok(_newsArticleService.GetActiveNews());
        }

        [HttpGet("active/{id}")]
        [AllowAnonymous]
        public IActionResult GetActiveById(string id)
        {
            var article = _newsArticleService.GetById(id);
            if (article == null || article.NewsStatus != true) return NotFound();
            return Ok(article);
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = "RequireAdminOrStaff")]
        public IActionResult GetAll()
        {
            return Ok(_newsArticleService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireAdminOrStaff")]
        public IActionResult GetById(string id)
        {
            var article = _newsArticleService.GetById(id);
            if (article == null) return NotFound();
            return Ok(article);
        }

        [HttpGet("search")]
        [Authorize(Policy = "RequireAdminOrStaff")]
        public IActionResult Search([FromQuery] string keyword = null)
        {
            return Ok(_newsArticleService.Search(keyword));
        }

        [HttpGet("created-by/{accountId:short}")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult GetByCreatedBy(short accountId)
        {
            return Ok(_newsArticleService.GetByCreatedBy(accountId));
        }

        [HttpGet("report")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetReportStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(_newsArticleService.GetReportStatistics(startDate, endDate));
        }

        [HttpGet("by-date-range")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(_newsArticleService.GetByDateRange(startDate, endDate));
        }

        [HttpPost]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult Create([FromBody] NewsArticleCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(dto.NewsArticleID))
            {
                dto.NewsArticleID = DateTime.Now.Ticks.ToString();
            }
            var currentAccountId = User.FindFirst("AccountID")?.Value;
            if (!dto.CreatedByID.HasValue && short.TryParse(currentAccountId, out var accId))
            {
                dto.CreatedByID = accId;
            }
            _newsArticleService.Insert(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.NewsArticleID }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult Update(string id, [FromBody] NewsArticleUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existing = _newsArticleService.GetById(id);
            if (existing == null) return NotFound();
            var currentAccountId = User.FindFirst("AccountID")?.Value;
            if (!dto.UpdatedByID.HasValue && short.TryParse(currentAccountId, out var accId))
            {
                dto.UpdatedByID = accId;
            }
            _newsArticleService.Update(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult Delete(string id)
        {
            var existing = _newsArticleService.GetById(id);
            if (existing == null) return NotFound();
            _newsArticleService.Delete(id);
            return Ok(new { message = "News article deleted successfully." });
        }
    }
}
