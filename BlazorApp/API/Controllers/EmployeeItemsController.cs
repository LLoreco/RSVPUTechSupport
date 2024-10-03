using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;

        public EmployeeItemsController(ApplicationDbContext context)
        {
            _context = context;
            _employeeService = new EmployeeService(_context);
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetTodoItems()
        {
            return _employeeService.GetAllEmployee();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetTodoItem(int id)
        {
            return _employeeService.GetEmployee(id);
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<Employee>> PostTodoItem(Employee item)
        {
            if (_employeeService.InsertRecord(item))
            {
                return CreatedAtAction(nameof(GetTodoItem), new { id = item.id }, item);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, Employee item)
        {
            if (_employeeService.UpdateRecord(item))
            {
                await _context.SaveChangesAsync();
            }
            

            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if (_employeeService.DeleteRecord(_employeeService.GetEmployee(id)))
            {
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.employees.Any(e => e.id == id);
        }
    }
}
