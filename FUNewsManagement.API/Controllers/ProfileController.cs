using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Models.DTOs;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireStaffRole")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly INewsArticleService _newsArticleService;

        public ProfileController(IProfileService profileService, INewsArticleService newsArticleService)
        {
            _profileService = profileService;
            _newsArticleService = newsArticleService;
        }

        private short GetCurrentAccountId()
        {
            var accountIdClaim = User.FindFirst("AccountID")?.Value;
            return short.TryParse(accountIdClaim, out var id) ? id : (short)0;
        }

        [HttpGet]
        public IActionResult GetProfile()
        {
            var id = GetCurrentAccountId();
            if (id == 0) return Unauthorized();
            var profile = _profileService.GetProfile(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut]
        public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = GetCurrentAccountId();
            if (id == 0) return Unauthorized();
            _profileService.UpdateProfile(id, dto);
            return Ok(new { message = "Profile updated successfully." });
        }

        [HttpGet("news-history")]
        public IActionResult GetNewsHistory()
        {
            var id = GetCurrentAccountId();
            if (id == 0) return Unauthorized();
            return Ok(_newsArticleService.GetByCreatedBy(id));
        }
    }
}
