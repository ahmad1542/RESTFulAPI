using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebApplication1.Controllers.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase {

        public CategoriesController(AppDbContext db) {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetCategories() {
            var catgs = await _db.Categories.ToListAsync();
            return Ok(catgs);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string category) {
            Category cat = new() { Name = category };
            await _db.Categories.AddAsync(cat);
            _db.SaveChanges();
            return Ok(cat);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category) {
            var cat = await _db.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
            if (cat == null)
                return NotFound($"Category ID {category.Id} not exists");
            cat.Name = category.Name;
            _db.SaveChanges();
            return Ok(cat);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategoryPatch([FromBody] JsonPatchDocument<Category> category, [FromRoute] int id) {
            var cat = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (cat == null)
                return NotFound($"Category ID {id} not exists");
            category.ApplyTo(cat);
            await _db.SaveChangesAsync();
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(int id) {
            var cat = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (cat == null)
                return NotFound($"Category ID {id} not exists");
            _db.Categories.Remove(cat);
            _db.SaveChanges();
            return Ok(cat);
        }

    }
}
