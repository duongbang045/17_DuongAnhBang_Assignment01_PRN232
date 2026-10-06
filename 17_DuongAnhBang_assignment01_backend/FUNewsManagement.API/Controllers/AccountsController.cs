using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using FUNewsManagement.Models;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetAll()
        {
            return Ok(_accountService.GetAll());
        }

        [HttpGet("{id:short}")]
        public IActionResult GetById(short id)
        {
            var account = _accountService.GetById(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword = null)
        {
            return Ok(_accountService.Search(keyword));
        }

        [HttpPost]
        public IActionResult Create([FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_accountService.GetByEmail(account.AccountEmail) != null)
            {
                return BadRequest(new { message = "Email already exists." });
            }
            _accountService.Insert(account);
            return CreatedAtAction(nameof(GetById), new { id = account.AccountID }, account);
        }

        [HttpPut("{id:short}")]
        public IActionResult Update(short id, [FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != account.AccountID) return BadRequest();
            var existing = _accountService.GetById(id);
            if (existing == null) return NotFound();
            _accountService.Update(account);
            return NoContent();
        }

        [HttpDelete("{id:short}")]
        public IActionResult Delete(short id)
        {
            var result = _accountService.Delete(id);
            if (!result.success)
            {
                return BadRequest(new { message = result.message });
            }
            return Ok(new { message = result.message });
        }
    }
}
