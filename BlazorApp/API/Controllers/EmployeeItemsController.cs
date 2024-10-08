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

        // GET: api/EmployeeItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = _employeeService.GetAllEmployee().Result;
            return Ok(employees);
        }

        // GET: api/EmployeeItems/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = _employeeService.GetEmployee(id).Result;
            return Ok(employee.Result);
        }

        // POST: api/EmployeeItems
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee item)
        {
            var result = await _employeeService.InsertRecord(item);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetEmployee), new { id = item.id }, item);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/EmployeeItems/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee item)
        {
            var result = await _employeeService.UpdateRecord(item);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        // DELETE: api/EmployeeItems/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.GetEmployee(id);
            var result = await _employeeService.DeleteRecord(employee.Result);
            if (result.IsSuccess)
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
