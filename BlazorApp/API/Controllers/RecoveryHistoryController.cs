using API.Data;
using API.Services;
using BlazorApp.Components.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RecoveryHistoryService _recoveryHistoryService;

        public RecoveryHistoryController(ApplicationDbContext context)
        {
            _context = context;
            _recoveryHistoryService = new RecoveryHistoryService(_context);
        }

        // GET: api/RecoveryHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecoveryHistory>>> GetRecoveryHistorys()
        {
            var RecoveryHistorys = await _recoveryHistoryService.GetALlRecoveryHistory();
            return Ok(RecoveryHistorys);
        }

        // GET: api/RecoveryHistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecoveryHistory>> GetRecoveryHistory(int id)
        {
            var RecoveryHistory = await _recoveryHistoryService.GetRecoveryHistory(id);
            return Ok(RecoveryHistory);
        }

        // POST: api/RecoveryHistory
        [HttpPost]
        public async Task<ActionResult<RecoveryHistory>> PostRecoveryHistory(RecoveryHistory item)
        {
            var result = await _recoveryHistoryService.InsertRecord(item, false);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetRecoveryHistory), new { id = item.id }, item);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/RecoveryHistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecoveryHistory(int id, RecoveryHistory item)
        {
            var result = await _recoveryHistoryService.UpdateRecord(item);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        // DELETE: api/RecoveryHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleRecoveryHistory(int id)
        {
            var RecoveryHistory = await _recoveryHistoryService.GetRecoveryHistory(id);
            var result = await _recoveryHistoryService.DeleteRecord(RecoveryHistory.Result);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.recoveryHistory.Any(e => e.id == id);
        }
    }
}
