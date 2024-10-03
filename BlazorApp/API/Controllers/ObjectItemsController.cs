using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObjectItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Objects>>> GetTodoItems()
        {
            return await _context.objects.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Objects>> GetTodoItem(int id)
        {
            var todoItem = await _context.objects.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<Objects>> PostTodoItem(Objects item)
        {
            _context.objects.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.id }, item);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, Objects item)
        {
            if (id != item.id)
            {
                return BadRequest("IDs do not match");
            }

            var existingItem = await _context.objects.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound("Item not found");
            }

            // Обновляем только те поля, которые действительно нужно обновить
            existingItem.object_name = item.object_name;
            existingItem.type = item.type;
            existingItem.buy_date = item.buy_date;
            existingItem.break_count = item.break_count;
            existingItem.recovery_date = item.recovery_date;
            existingItem.room_number = item.room_number;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound("Item not found after update attempt");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.objects.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.objects.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.objects.Any(e => e.id == id);
        }
    }
}
