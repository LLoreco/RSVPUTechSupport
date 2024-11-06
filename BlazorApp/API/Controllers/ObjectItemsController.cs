using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Data.Entity.Core.Objects;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ObjectsService _objectService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ObjectItemsController(ApplicationDbContext context)
        {
            _context = context;
            _objectService = new ObjectsService(_context);
        }

        // GET: api/ObjectItems/GetObjects
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("GetObjects")]
        public async Task<ActionResult<IEnumerable<Objects>>> GetObjects()
        {
            try
            {
                var objects = _objectService.GetObjects().Result;
                if (objects.Count > 0)
                {
                    _logger.Info("Получил все объекты через GET запрос");
                    return Ok(JsonSerializer.Serialize(objects, new JsonSerializerOptions { WriteIndented = true }));
                }
                else
                {
                    return StatusCode(404, "Объекты не найдены");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Произошла ошибка при получении списка объектов.");
                return StatusCode(500, "Произошла ошибка при получении списка объектов. Попробуйте позже.");
            }

        }

        // GET:  api/ObjectItems/GetObjects/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("GetObjects/{id}")]
        public async Task<ActionResult<Objects>> GetObject(int id)
        {
            try
            {
                var objects = await _objectService.GetObject(id);
                if (objects.IsSuccess)
                {
                    _logger.Info($"Получил объект {id} через GET запрос");
                    return Ok(JsonSerializer.Serialize(objects));
                }
                else
                {
                    return StatusCode(404, "Объект не найден");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при получении объекта {id} ");
                return StatusCode(500, $"Произошла ошибка при получении объекта {id}. Попробуйте позже.");
            }

        }

        // POST: api/ObjectItems/AddObject
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("AddObject")]
        public async Task<ActionResult<Objects>> PostObject(Objects item)
        {
            try
            {
                var result = await _objectService.InsertRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Добавил объект {item.id} через POST запрос");
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "Объект не добавлен");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при добавлении объекта {item.id} ");
                return StatusCode(500, $"Произошла ошибка при добавлении объекта {item.id}. Попробуйте позже.");
            }
           
        }

        // PUT: api/ObjectItems/UpdateObjects/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("UpdateObjects/{id}")]
        public async Task<IActionResult> PutObject(int id, Objects item)
        {
            try
            {
                var result = await _objectService.UpdateRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Обновил объект {item.id} через PUT запрос");
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "Объект не найден");
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при обновлении объекта {item.id} ");
                return StatusCode(500, $"Произошла ошибка при обновлении объекта {item.id}. Попробуйте позже.");
            }
            
        }

        // DELETE: api/ObjectItems/DeleteObjects/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("DeleteObjects/{id}")]
        public async Task<IActionResult> DeleteObject(int id)
        {
            try
            {
                var objectsResult = await _objectService.GetObject(id);
                if (!objectsResult.IsSuccess)
                {
                    return StatusCode(404, "Сотрудник не найден.");
                }
                else
                {
                    var objects = objectsResult.Result;
                    var result = await _objectService.DeleteRecord(objects);
                    if (result.IsSuccess)
                    {
                        _logger.Info($"Удалил объект {id} через DELETE запрос");
                        await _context.SaveChangesAsync();

                        return Ok(JsonSerializer.Serialize(objects));
                    }
                    return StatusCode(500, $"Произошла ошибка при удалении объекта {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при удалении объекта {id} ");
                return StatusCode(500, $"Произошла ошибка при удалении объекта {id}. Попробуйте позже.");
            }
        }
    }
}
