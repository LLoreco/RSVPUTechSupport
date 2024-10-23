using API.Data;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace BlazorApp.Components.Services
{
    public class WorkService : IWorkService
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public WorkService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Work>> GetWorks()
        {
            try
            {
                return await _dbContext.work.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы WORK в WorkService");
                return new List<Work>();
            }
        }
        public async Task<TaskResult<Work>> GetWork(int id)
        {
            try
            {
                var work = await _dbContext.work.FindAsync(id);
                return new TaskResult<Work>
                {
                    IsSuccess = true,
                    Result = work
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы WORK в WorkService");
                return new TaskResult<Work>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }
        public async Task<TaskResult<bool>> InsertRecord(Work work)
        {
            try
            {
                if (work.from_whom_id != 0 && work.employee_id == 0)
                {
                    var availableEmployee = _dbContext.employees.FirstOrDefault(e => !e.status);
                    work.employee_id = availableEmployee.id;
                    work.image = FindEmployee(work.employee_id);
                    work.status = "Ожидаем подтверждения";
                    work.work_number = GetNextAvailableWorkNumber();
                    work.send_time = DateTime.UtcNow;
                    work.time_limit = work.time_limit.UtcDateTime;
                    work.Employee = _dbContext.employees.FirstOrDefault(e => e.id == work.employee_id);
                    work.FromWhom = _dbContext.employees.FirstOrDefault(e => e.id == work.from_whom_id);
                }
                else
                {
                    work.image = FindEmployee(work.employee_id);
                    ConvertTime(work);
                    work.Employee = _dbContext.employees.FirstOrDefault(e => e.id == work.employee_id);
                    work.FromWhom = _dbContext.employees.FirstOrDefault(e => e.id == work.from_whom_id);
                }
                _dbContext.work.Add(work);
                _dbContext.SaveChanges();
                CreateRecordToRecoveryHistory(work);
                UpdateObjectBreakCount(work);
                _logger.Info("Создана новая запись и сохранена в таблицу WORK");
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу WORK в WorkService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
            
        }

        public async Task<TaskResult<Work>> EditRecord(int workID)
        {
            try
            {
                var work = await _dbContext.work.FindAsync(workID);
                return new TaskResult<Work>
                {
                    IsSuccess = true,
                    Result = work
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу WORK в WorkService");
                return new TaskResult<Work>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }

        public async Task<TaskResult<bool>> UpdateRecord(Work workUpdate)
        {
            try
            {
                var workRecordUpdate = await _dbContext.work.FindAsync(workUpdate.id);
                if (workRecordUpdate != null)
                {
                    workRecordUpdate.work_number = workUpdate.work_number;
                    workRecordUpdate.description = workUpdate.description;
                    workRecordUpdate.send_time = workUpdate.send_time.ToUniversalTime();
                    workRecordUpdate.time_limit = workUpdate.time_limit.ToUniversalTime();
                    workRecordUpdate.total_time = workUpdate.total_time.ToUniversalTime();
                    workRecordUpdate.status = workUpdate.status;

                    var availableEmployee = _dbContext.employees.FirstOrDefault(e => e.id == workUpdate.employee_id);
                    var obj = _dbContext.objects.FirstOrDefault(o => o.id ==  workUpdate.object_id);
                    if (availableEmployee != null && obj != null)
                    {
                        workRecordUpdate.employee_id = availableEmployee.id;
                        workRecordUpdate.image = FindEmployee(workRecordUpdate.employee_id);
                        workRecordUpdate.object_id = obj.id;
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        workUpdate.employee_id = workRecordUpdate.employee_id;
                        workRecordUpdate.image = "ОШИБКА";
                        await _dbContext.SaveChangesAsync();
                    }
                    _logger.Info("Запись обновлена");
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
                _logger.Error(ex, "Не удалось изменить данные в таблице WORK в WorkService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }

        public async Task<TaskResult<bool>> DeleteRecord(Work workDelete)
        {
            try
            {
                var workRecordDelete = await _dbContext.work.FindAsync(workDelete.id);
                if (workRecordDelete != null)
                {
                    _dbContext.work.Remove(workRecordDelete);
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
                _logger.Info("Запись удалена");
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось удалить данные из таблицы WORK в WorkService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
            
        }

        public string FindEmployee(int id, string fromWhom = null)
        {
            try
            {
                var employee = _dbContext.employees.FirstOrDefault(e => e.id == id);
                if (employee != null)
                {
                    _logger.Info("Работник найден");
                    return $"{employee.first_name} {employee.last_name}";
                }
                else
                {
                    return "ОШИБКА";
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось найти работника по id из таблицы WORK в WorkService");
                return "ОШИБКА";
            }
            
        }

        public int GetNextAvailableWorkNumber()
        {
            var maxWorkNumber = _dbContext.work.Max(w => w.work_number);
            return maxWorkNumber + 1;
        }
        private void ConvertTime(Work work)
        {
            work.send_time = DateTime.UtcNow + TimeSpan.FromHours(5);
            work.time_limit = work.time_limit.UtcDateTime;
        }
        private void CreateRecordToRecoveryHistory(Work work)
        {
            RecoveryHistory history = new RecoveryHistory();
            history.id = work.id;
            history.description = work.description;
            history.employee_id = work.employee_id;
            history.recovery_date = work.send_time.UtcDateTime;
            history.total_time = work.total_time.UtcDateTime;
            history.object_id = work.object_id;
            var recoveryHistoryService = new RecoveryHistoryService(_dbContext);
            recoveryHistoryService.InsertRecord(history, true);
        }
        private void UpdateObjectBreakCount(Work work)
        {
            Objects objectDB = _dbContext.objects.FindAsync(work.object_id).Result;
            var objectService = new ObjectsService(_dbContext);
            objectService.UpdateRecord(objectDB);
        }
    }
}
