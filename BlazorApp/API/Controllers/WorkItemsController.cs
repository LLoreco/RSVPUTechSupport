using API.Data;
using BlazorApp.Components.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkService _workService;

        public WorkItemsController(ApplicationDbContext context)
        {
            _context = context;
            _workService = new WorkService(_context);
        }

        // GET: api/WorkItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorks()
        {
            var works = await _workService.GetWorks();
            return Ok(works);
        }

        // GET: api/WorkItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Work>> GetWork(int id)
        {
            var work = await _workService.GetWork(id);
            return Ok(work);
        }

        // POST: api/WorkItems
        [HttpPost]
        public async Task<ActionResult<Work>> PostWork(Work item)
        {
            var result = await _workService.InsertRecord(item);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetWork), new { id = item.id }, item);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/WorkItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWork(int id, Work item)
        {
            var result = await _workService.UpdateRecord(item);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        // DELETE: api/WorkItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleWork(int id)
        {
            var work = await _workService.GetWork(id);
            var result = await _workService.DeleteRecord(work.Result);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.work.Any(e => e.id == id);
        }
    }
}
