using API.Data;
using API.Services;
using BlazorApp.Components.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RecoveryHistoryService _recoveryHistoryService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public RecoveryHistoryController(ApplicationDbContext context)
        {
            _context = context;
            _recoveryHistoryService = new RecoveryHistoryService(_context);
        }

        // GET: api/RecoveryHistory/GetRecoveryHistory
        [HttpGet("GetRecoveryHistory")]
        public async Task<ActionResult<IEnumerable<RecoveryHistory>>> GetRecoveryHistorys()
        {
            try
            {
                var recoveryHistorys = await _recoveryHistoryService.GetALlRecoveryHistory();
                if (recoveryHistorys.Count > 0)
                {
                    _logger.Info("Получил всю историю починок через GET запрос");
                    return Ok(JsonSerializer.Serialize(recoveryHistorys));
                }
                else
                {
                    return StatusCode(404, "История починок не найдены.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Произошла ошибка при получении списка истории починок.");
                return StatusCode(500, "Произошла ошибка при получении списка истории починок. Попробуйте позже.");
            }
        }

        // GET: api/RecoveryHistory/GetRecoveryHistory/id
        [HttpGet("GetRecoveryHistory/{id}")]
        public async Task<ActionResult<RecoveryHistory>> GetRecoveryHistory(int id)
        {
            try
            {
                var recoveryHistory = await _recoveryHistoryService.GetRecoveryHistory(id);
                if (recoveryHistory.IsSuccess)
                {
                    _logger.Info($"Получил всю историю починок по объекту {id} через GET запрос");
                    return Ok(JsonSerializer.Serialize(recoveryHistory));
                }
                else
                {
                    return StatusCode(404, "История починок не найдена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при получении истории починок объекта {id}.");
                return StatusCode(500, $"Произошла ошибка при получении истории починок объекта {id}. Попробуйте позже.");
            }
        }

        // POST: api/RecoveryHistory/AddRecoveryHistory
        [HttpPost("AddRecoveryHistory")]
        public async Task<ActionResult<RecoveryHistory>> PostRecoveryHistory(RecoveryHistory item)
        {
            try
            {
                var result = await _recoveryHistoryService.InsertRecord(item, false);
                if (result.IsSuccess)
                {
                    _logger.Info($"Добавил историю починок объекта {item.id} через POST запрос");
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "История починок не добавлена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при добавлении истории починок объекта {item.id}.");
                return StatusCode(500, $"Произошла ошибка при добавлении истории починок объекта {item.id}. Попробуйте позже.");
            }
            
        }

        // PUT: api/RecoveryHistory/UpdateRecoveryHistory/id
        [HttpPut("UpdateRecoveryHistory/{id}")]
        public async Task<IActionResult> PutRecoveryHistory(int id, RecoveryHistory item)
        {
            try
            {
                var result = await _recoveryHistoryService.UpdateRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Обновил историю починок объекта {item.id} через PUT запрос");
                    await _context.SaveChangesAsync();
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "История починок не обновлена.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при обновлении истории починок объекта {item.id}.");
                return StatusCode(500, $"Произошла ошибка при обновлении истории починок объекта {item.id}. Попробуйте позже.");
            }
            
        }

        // DELETE: api/RecoveryHistory/DeleteRecoveryHistory/id
        [HttpDelete("DeleteRecoveryHistory/{id}")]
        public async Task<IActionResult> DeteleRecoveryHistory(int id)
        {
            try
            {
                var recoveryHistoryResult = await _recoveryHistoryService.GetRecoveryHistory(id);
                if (!recoveryHistoryResult.IsSuccess)
                {
                    return StatusCode(404, "История починок не удалена.");
                }
                else
                {
                    var recoveryHistory = recoveryHistoryResult.Result;
                    var deleteResult = await _recoveryHistoryService.DeleteRecord(recoveryHistory);
                    if (deleteResult.IsSuccess)
                    {
                        _logger.Info($"Удалил историю починок объекта {id} через PUT запрос");
                        await _context.SaveChangesAsync();
                        return Ok(JsonSerializer.Serialize(recoveryHistory));
                    }
                    return StatusCode(500, $"Произошла ошибка при удалении истории починок объекта {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при удалении истории починок объекта {id}.");
                return StatusCode(500, $"Произошла ошибка при удалении истории починок объекта {id}. Попробуйте позже.");
            }
        }

        private bool TodoItemExists(int id)
        {
            return _context.recoveryHistory.Any(e => e.id == id);
        }
    }
}
