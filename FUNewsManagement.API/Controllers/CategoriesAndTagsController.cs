using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using FUNewsManagement.Models;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireStaffRole")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{id:short}")]
        public IActionResult GetById(short id)
        {
            var category = _categoryService.GetById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword = null)
        {
            return Ok(_categoryService.Search(keyword));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _categoryService.Insert(category);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryID }, category);
        }

        [HttpPut("{id:short}")]
        public IActionResult Update(short id, [FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != category.CategoryID) return BadRequest();
            var existing = _categoryService.GetById(id);
            if (existing == null) return NotFound();
            _categoryService.Update(category);
            return NoContent();
        }

        [HttpDelete("{id:short}")]
        public IActionResult Delete(short id)
        {
            var result = _categoryService.Delete(id);
            if (!result.success)
            {
                return BadRequest(new { message = result.message });
            }
            return Ok(new { message = result.message });
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireStaffRole")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_tagService.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var tag = _tagService.GetById(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword = null)
        {
            return Ok(_tagService.Search(keyword));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Tag tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _tagService.Insert(tag);
            return CreatedAtAction(nameof(GetById), new { id = tag.TagID }, tag);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Tag tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != tag.TagID) return BadRequest();
            var existing = _tagService.GetById(id);
            if (existing == null) return NotFound();
            _tagService.Update(tag);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = _tagService.GetById(id);
            if (existing == null) return NotFound();
            _tagService.Delete(id);
            return Ok(new { message = "Tag deleted successfully." });
        }
    }
}
