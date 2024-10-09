using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace API.Services
{
    public class RecoveryHistoryService : IRecoveryHistoryService
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public RecoveryHistoryService(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<RecoveryHistory>> GetALlRecoveryHistory()
        {
            try
            {
                return await _dbContext.recoveryHistory.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы RECOVERYHISTORY в RecoveryHistoryService");
                return new List<RecoveryHistory>();
            }
        }
        public async Task<TaskResult<RecoveryHistory>> GetRecoveryHistory(int id)
        {
            try
            {
                var recoveryHistory = await _dbContext.recoveryHistory.FindAsync(id);
                return new TaskResult<RecoveryHistory>
                {
                    IsSuccess = true,
                    Result = recoveryHistory
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы RECOVERYHISTORY в RecoveryHistoryService");
                return new TaskResult<RecoveryHistory>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }
        public async Task<TaskResult<bool>> InsertRecord(RecoveryHistory recoveryHistory, bool fromWork = false)
        {
            try
            {
                Employee employee;
                Objects objectDB;
                Work work;
                if (!fromWork)
                {
                    employee = _dbContext.employees.FirstOrDefault(e => e.id == recoveryHistory.employee_id);
                    objectDB = _dbContext.objects.FirstOrDefault(o => o.id == recoveryHistory.object_id);
                    work = _dbContext.work.FirstOrDefault(w => employee.id == w.employee_id);
                    if (employee != null && work != null && objectDB != null)
                    {
                        recoveryHistory.description = work.description;
                        recoveryHistory.total_time = DateTime.SpecifyKind(work.total_time, DateTimeKind.Utc);
                        recoveryHistory.recovery_date = DateTime.SpecifyKind(work.send_time, DateTimeKind.Utc);

                        recoveryHistory.employee_id = employee.id;

                        recoveryHistory.object_id = objectDB.id;
                    }
                }
                
                
                _dbContext.recoveryHistory.Add(recoveryHistory);
                await _dbContext.SaveChangesAsync();
                _logger.Info("Создана новая запись и сохранена в таблицу RECOVERYHISTORY");
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу RECOVERYHISTORY в RecoveryHistoryService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
        public async Task<TaskResult<RecoveryHistory>> EditRecord(int recoveryHistoryId)
        {
            try
            {
                var recoveryHistory = await _dbContext.recoveryHistory.FindAsync(recoveryHistoryId);
                return new TaskResult<RecoveryHistory>
                {
                    IsSuccess = true,
                    Result = recoveryHistory
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу RECOVERYHISTORY в RecoveryHistoryService");
                return new TaskResult<RecoveryHistory>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }
        public async Task<TaskResult<bool>> UpdateRecord(RecoveryHistory recoveryHistoryUpdate)
        {
            try
            {
                var recoveryHistoryRecordUpdate = await _dbContext.recoveryHistory.FindAsync(recoveryHistoryUpdate.id);
                if (recoveryHistoryRecordUpdate != null)
                {
                    recoveryHistoryRecordUpdate.id = recoveryHistoryUpdate.id;
                    recoveryHistoryRecordUpdate.description = recoveryHistoryUpdate.description;
                    recoveryHistoryRecordUpdate.employee_id = recoveryHistoryUpdate.employee_id;
                    recoveryHistoryRecordUpdate.total_time = recoveryHistoryUpdate.total_time.ToUniversalTime();
                    recoveryHistoryRecordUpdate.recovery_date = recoveryHistoryRecordUpdate.recovery_date.ToUniversalTime();
                    recoveryHistoryRecordUpdate.object_id = recoveryHistoryUpdate.object_id;
                    _logger.Info("Запись обновлена");
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
                _logger.Error(ex, "Не удалось обновить данные в таблице RECOVERYHISTORY в RecoveryHistoryService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
        public async Task<TaskResult<bool>> DeleteRecord(RecoveryHistory recoveryHistoryDelete)
        {
            try
            {
                var recoveryHistoryRecordDelete = await _dbContext.recoveryHistory.FindAsync(recoveryHistoryDelete.id);
                if (recoveryHistoryRecordDelete != null)
                {
                    _dbContext.recoveryHistory.Remove(recoveryHistoryRecordDelete);
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
                _logger.Error(ex, "Не удалось удалить данные из таблицы RECOVERYHISTORY в RecoveryHistoryService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
    }
}
