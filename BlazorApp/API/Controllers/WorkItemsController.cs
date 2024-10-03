using API.Data;
using BlazorApp.Components.Services;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<Work>>> GetTodoItems()
        {
            return _workService.GetWorks();
        }

        // GET: api/WorkItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Work>> GetTodoItem(int id)
        {
            return _workService.GetWork(id);
        }

        // POST: api/WorkItems
        [HttpPost]
        public async Task<ActionResult<Work>> PostTodoItem(Work item)
        {
            if (_workService.InsertRecord(item))
            {
                return CreatedAtAction(nameof(GetTodoItem), new { id = item.id }, item);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/WorkItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, Work item)
        {
            if (_workService.UpdateRecord(item))
            {
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        // DELETE: api/WorkItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if (_workService.DeleteRecord(_workService.GetWork(id)))
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
