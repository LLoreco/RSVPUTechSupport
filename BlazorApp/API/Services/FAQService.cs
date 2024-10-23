using NLog;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class FAQService
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public FAQService (ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<FAQ>> GetAllFAQ()
        {
            try
            {
                return await _dbContext.faq.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы FAQ в FAQService");
                return new List<FAQ>();
            }
        }
        public async Task<TaskResult<FAQ>> GetFAQ(int id)
        {
            try
            {
                var faq = await _dbContext.faq.FindAsync(id);
                return new TaskResult<FAQ>
                {
                    IsSuccess = true,
                    Result = faq
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы FAQ в FAQService");
                return new TaskResult<FAQ>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }
        public async Task<TaskResult<bool>> InsertRecord(FAQ faq)
        {
            try
            {
                _dbContext.faq.Add(faq);
                await _dbContext.SaveChangesAsync();
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка добавления записи в FAQService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
        public async Task<TaskResult<FAQ>> EditRecord(int faqid)
        {
            try
            {
                var faq = await _dbContext.faq.FindAsync(faqid);
                return new TaskResult<FAQ>
                {
                    IsSuccess = true,
                    Result = faq
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка редактирования записи в FAQService");
                return new TaskResult<FAQ>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }
        public async Task<TaskResult<bool>> UpdateRecord(FAQ faqUpdate)
        {
            try
            {
                var faqRecordUpdate = await _dbContext.faq.FindAsync(faqUpdate.id);
                if (faqRecordUpdate != null)
                {
                    faqRecordUpdate.id = faqUpdate.id;
                    faqRecordUpdate.name = faqUpdate.name;
                    faqRecordUpdate.description = faqUpdate.description;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new TaskResult<bool>
                    {
                        IsSuccess = false,
                        Result = false
                    };
                }
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка обновления записи в FAQService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
        public async Task<TaskResult<bool>> DeleteRecord(FAQ faqDelete)
        {
            try
            {
                var faqRecordDelete = await _dbContext.faq.FindAsync(faqDelete.id);
                if (faqRecordDelete != null)
                {
                    _dbContext.Remove(faqRecordDelete);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new TaskResult<bool>
                    {
                        IsSuccess = false,
                        Result = false
                    };
                }
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка удвления записи в FAQService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
    }
}
