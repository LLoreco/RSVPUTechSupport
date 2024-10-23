using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Services;
using NLog;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FAQService _faqService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public FAQController(ApplicationDbContext context)
        {
            _context = context;
            _faqService = new FAQService(_context);
        }

        // GET: api/FAQ/GetFAQ
        [HttpGet("GetFAQ")]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQs()
        {
            try
            {
                var faqs = _faqService.GetAllFAQ().Result;

                if (faqs.Count > 0)
                {
                    _logger.Info("Получил всех FAQ через GET запрос");

                    return Ok(JsonSerializer.Serialize(faqs));
                }
                else
                {
                    return StatusCode(404, "FAQ не найдены.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Произошла ошибка при получении списка FAQ.");
                return StatusCode(500, "Произошла ошибка при получении списка FAQ. Попробуйте позже.");
            }
        }

        // GET: api/FAQ/GetFAQ/id
        [HttpGet("GetFAQ/{id}")]
        public async Task<ActionResult<FAQ>> GetFAQ(int id)
        {
            try
            {
                var faq = _faqService.GetFAQ(id).Result;
                if (faq.IsSuccess)
                {
                    _logger.Info($"Получил FAQ {id} через GET запрос");
                    return Ok(JsonSerializer.Serialize(faq));
                }
                else
                {
                    return StatusCode(404, "FAQ не найдено.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при получении FAQ {id}.");
                return StatusCode(500, $"Произошла ошибка при получении FAQ {id}. Попробуйте позже.");
            }
        }

        // POST: api/FAQ/AddFAQ
        [HttpPost("AddFAQ")]
        public async Task<ActionResult<FAQ>> PostFAQ(FAQ item)
        {
            try
            {
                var result = await _faqService.InsertRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"Добавил FAQ {item.id} через POST запрос");
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "FAQ не добавлено.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при добавлении FAQ {item.id}.");
                return StatusCode(500, $"Произошла ошибка при добавлении FAQ {item.id}. Попробуйте позже.");
            }
            
        }

        // PUT: api/FAQ/UpdateFAQ/id
        [HttpPut("UpdateFAQ/{id}")]
        public async Task<IActionResult> PutFAQ(int id, FAQ item)
        {
            try
            {
                var result = await _faqService.UpdateRecord(item);
                if (result.IsSuccess)
                {
                    _logger.Info($"обновлено FAQ {item.id} через PUT запрос");
                    await _context.SaveChangesAsync();
                    return Ok(JsonSerializer.Serialize(item));
                }
                else
                {
                    return StatusCode(404, "FAQ не обновлено.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при обновлении FAQ {item.id}.");
                return StatusCode(500, $"Произошла ошибка при обновлении FAQ {item.id}. Попробуйте позже.");
            }
        }

        // DELETE: api/FAQ/DeleteFAQ/id
        [HttpDelete("DeleteFAQ/{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                var faqResult = await _faqService.GetFAQ(id);
                if (!faqResult.IsSuccess)
                {
                    return StatusCode(404, "FAQ не обновлено.");
                }
                else
                {
                    var faq = faqResult.Result;
                    var deleteResult = await _faqService.DeleteRecord(faq);
                    if (deleteResult.IsSuccess)
                    {
                        _logger.Info($"Удалено FAQ {id} через DELETE запрос");
                        return Ok(JsonSerializer.Serialize(faq));
                    }
                    return StatusCode(500, $"Произошла ошибка при удалении сотрудника {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Произошла ошибка при удалении FAQ {id}.");
                return StatusCode(500, $"Произошла ошибка при удалении FAQ {id}. Попробуйте позже.");
            }
        }

        private bool TodoItemExists(int id)
        {
            return _context.faq.Any(e => e.id == id);
        }
    }
}
