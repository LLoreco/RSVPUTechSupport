using API.Data;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ObjectsService _objectService;

        public ObjectItemsController(ApplicationDbContext context)
        {
            _context = context;
            _objectService = new ObjectsService(_context);
        }

        // GET: api/ObjectItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Objects>>> GetObjects()
        {
            var objects = _objectService.GetObjects().Result;
            return Ok(objects);
        }

        // GET: api/ObjectItems/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Objects>> GetObject(int id)
        {
            var objects = await _objectService.GetObject(id);
            return Ok(objects);
        }

        // POST: api/ObjectItems
        [HttpPost]
        public async Task<ActionResult<Objects>> PostObject(Objects item)
        {
            var result = await _objectService.InsertRecord(item);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetObject), new { id = item.id }, item);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/ObjectItems/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObject(int id, Objects item)
        {
            var result = await _objectService.UpdateRecord(item);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }
            
            return NoContent();
        }

        // DELETE: api/ObjectItems/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObject(int id)
        {
            var objects = await _objectService.GetObject(id);
            var result = await _objectService.DeleteRecord(objects.Result);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
