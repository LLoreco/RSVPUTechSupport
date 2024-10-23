using API.Data;
using BlazorApp.Components.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkService _workService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WorkItemsController(ApplicationDbContext context)
        {
            _context = context;
            _workService = new WorkService(_context);
        }

        // GET: api/WorkItems/GetWork
        [HttpGet("GetWork")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorks()
        {
            try
            {
                var works = await _workService.GetWorks();
                if (works.Count > 0)
                {
                    _logger.Info("Получил всю работу через GET запрос");
                    return Ok(JsonSerializer.Serialize(works));
                }
                else
                {
                    return StatusCode(404, "Работа не найдена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Произошла ошибка при получении списка работ.");
                return StatusCode(500, "Произошла ошибка при получении списка работ. Попробуйте позже.");
            }
        }

        // GET: api/WorkItems/GetWork/id
        [HttpGet("GetWork/{id}")]
        public async Task<ActionResult<Work>> GetWork(int id)
        {
            try
            {
                var work = await _workService.GetWork(id);
                if (work.IsSuccess)
                {
                    _logger.Info($"Получил работу {id} через GET запрос");
                    return Ok(JsonSerializer.Serialize(work));
                }
                else
                {
                    return StatusCode(404, "Работа не найдена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при получении работы {id}.");
                return StatusCode(500, $"Произошла ошибка при получении работы {id}. Попробуйте позже.");
            }
        }

        // POST: api/WorkItems/AddWork
        [HttpPost("AddWork")]
        public async Task<ActionResult<Work>> PostWork(Work item)
        {
            try
            {
                var result = await _workService.InsertRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Добавил работу {item.id} через POST запрос");
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "Работа не добавлена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при добавлении работы {item.id}.");
                return StatusCode(500, $"Произошла ошибка при добавлении работы {item.id}. Попробуйте позже.");
            }
        }

        // PUT: api/WorkItems/UpdateWork/id
        [HttpPut("UpdateWork/{id}")]
        public async Task<IActionResult> PutWork(int id, Work item)
        {
            try
            {
                var result = await _workService.UpdateRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Добавил работу {item.id} через PUT запрос");
                    await _context.SaveChangesAsync();
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "Работа не обновлена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при обновлении работы {item.id}.");
                return StatusCode(500, $"Произошла ошибка при обновлении работы {item.id}. Попробуйте позже.");
            }
        }

        // DELETE: api/WorkItems/DeleteWork/id
        [HttpDelete("DeleteWork/{id}")]
        public async Task<IActionResult> DeteleWork(int id)
        {
            try
            {
                var workResult = await _workService.GetWork(id);

                if (!workResult.IsSuccess)
                {
                    return StatusCode(404, "Работа не удалена.");
                }
                else
                {
                    var work = workResult.Result;
                    var deleteResult = await _workService.DeleteRecord(work);
                    if (deleteResult.IsSuccess)
                    {
                        _logger.Info($"Удалил работу {id} через DELETE запрос");
                        await _context.SaveChangesAsync();
                        return Ok(JsonSerializer.Serialize(work));
                    }
                    return StatusCode(500, $"Произошла ошибка при удалении работы {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при удалении работы {id}.");
                return StatusCode(500, $"Произошла ошибка при удалении работы {id}. Попробуйте позже.");
            }
        }

        private bool TodoItemExists(int id)
        {
            return _context.work.Any(e => e.id == id);
        }
    }
}
