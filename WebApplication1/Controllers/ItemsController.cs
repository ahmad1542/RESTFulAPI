using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Models;
using WebApplication1.Models;

namespace WebApplication1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase {

        public ItemsController(AppDbContext db) {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> AllItems() {
            var it = await _db.Categories.ToListAsync();
            return Ok(it);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> AllItems(int id) {
            var it = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (it == null)
                return NotFound($"Item code {id} not exists");
            return Ok(it);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] mdlItem mdl) {
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var item = new Item {
                Name = mdl.Name,
                Price = mdl.Price,
                Notes = mdl.Notes,
                CategoryId = mdl.CategoryId,
                Image = stream.ToArray()
            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }


    }
}
