using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Services;
using NLog;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EmployeeItemsController(ApplicationDbContext context)
        {
            _context = context;
            _employeeService = new EmployeeService(_context);
        }

        // GET: api/EmployeeItems/GetEmployee
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("GetEmployee")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployee();

                if (employees.Count > 0)
                {
                    _logger.Info("Получил всех работников через GET запрос");

                    return Ok(JsonSerializer.Serialize(employees));
                }
                else
                {
                    return StatusCode(404, "Сотрудники не найдены.");
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Произошла ошибка при получении списка сотрудников.");
                return StatusCode(500, "Произошла ошибка при получении списка сотрудников. Попробуйте позже.");
            }
        }

        // GET: api/EmployeeItems/GetEmployee/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("GetEmployee/{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployee(id);

                if (employee.IsSuccess)
                {
                    _logger.Info($"Получил работника {id} через GET запрос");
                    return Ok(JsonSerializer.Serialize(employee));
                }
                else
                {
                    return StatusCode(404, "Сотрудник не найден.");
                }
               
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при получении сотрудника {id} сотрудника.");
                return StatusCode(500, $"Произошла ошибка при получении сотрудникa {id}. Попробуйте позже.");
            }

        }

        // POST: api/EmployeeItems/AddEmployee
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("AddEmployee")]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee item)
        {
            try
            {
                var result = await _employeeService.InsertRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Добавил работника {item.id} через POST запрос");
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "Сотрудник не добавлен.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при добавлении сотрудника {item.id} сотрудника.");
                return StatusCode(500, $"Произошла ошибка при добавлении сотрудникa {item.id}. Попробуйте позже.");
            }
        }

        // PUT: api/EmployeeItems/UpdateEmployee/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] Employee item)
        {
            try
            {
                var result = await _employeeService.UpdateRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Обновил работника {item.id} через PUT запрос");
                    await _context.SaveChangesAsync();

                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "Сотрудник не найден.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при обновлении сотрудника {item.id} сотрудника.");
                return StatusCode(500, $"Произошла ошибка при обновлении сотрудникa {item.id}. Попробуйте позже.");
            }
            
        }

        // DELETE: api/EmployeeItems/DeleteEmployee/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employeeResult = await _employeeService.GetEmployee(id);

                if (!employeeResult.IsSuccess)
                {
                    return StatusCode(404, "Сотрудник не найден.");
                }
                else
                {
                    var employee = employeeResult.Result;

                    var deleteResult = await _employeeService.DeleteRecord(employee);

                    if (deleteResult.IsSuccess)
                    {
                        _logger.Info($"Удалил работника {id} через DELETE запрос");
                        await _context.SaveChangesAsync();

                        return Ok(JsonSerializer.Serialize(employee));
                    }
                    return StatusCode(500, $"Произошла ошибка при удалении сотрудника {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при удалении сотрудника {id} сотрудника.");
                return StatusCode(500, $"Произошла ошибка при удалении сотрудникa {id}. Попробуйте позже.");
            }
        }

    }
}
