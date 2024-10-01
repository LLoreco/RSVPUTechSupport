using BlazorApp.Components.Data;
using BlazorApp.Components.Interfaces;
using BlazorApp.Components.Model;
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

        public List<Work> GetWorks()
        {
            try
            {
                return _dbContext.work.OrderBy(e => e.id).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы WORK в WorkService");
                return new List<Work>();
            }
        }

        public bool InsertRecord(Work work)
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
                    work.time_limit = DateTime.SpecifyKind(work.time_limit, DateTimeKind.Utc);
                }
                else
                {
                    work.image = FindEmployee(work.employee_id);
                    ConvertTime(work);
                }
                _dbContext.work.Add(work);
                _dbContext.SaveChanges();
                _logger.Info("Создана новая запись и сохранена в таблицу WORK");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу WORK в WorkService");
                return false;
            }
            
        }

        public Work EditRecord(int workID)
        {
            try
            {
                return _dbContext.work.FirstOrDefault(u => u.id == workID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу WORK в WorkService");
                return new Work();
            }
        }

        public bool UpdateRecord(WorkDataModel workUpdate)
        {
            try
            {
                var workRecordUpdate = _dbContext.work.Include(w => w.Employee).FirstOrDefault(u => u.id == workUpdate.Id);
                if (workRecordUpdate != null)
                {
                    workRecordUpdate.work_number = workUpdate.workNumber;
                    workRecordUpdate.description = workUpdate.description;
                    workRecordUpdate.send_time = workUpdate.sendTime;
                    workRecordUpdate.time_limit = workUpdate.timeLimit;
                    workRecordUpdate.total_time = workUpdate.totalTime;
                    workRecordUpdate.status = workUpdate.status;

                    var availableEmployee = _dbContext.employees.FirstOrDefault(e => e.id == workUpdate.employeeId);
                    if (availableEmployee != null)
                    {
                        workRecordUpdate.employee_id = availableEmployee.id;
                        workRecordUpdate.image = FindEmployee(workRecordUpdate.employee_id);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        workUpdate.employeeId = workRecordUpdate.employee_id;
                        workRecordUpdate.image = "ОШИБКА";
                        _dbContext.SaveChanges();
                    }
                    _logger.Info("Запись обновлена");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось изменить данные в таблице WORK в WorkService");
                return false;
            }
        }

        public bool DeleteRecord(WorkDataModel workDelete)
        {
            try
            {
                var workRecordDelete = _dbContext.work.FirstOrDefault(u => u.id == workDelete.Id);
                if (workRecordDelete != null)
                {
                    _dbContext.work.Remove(workRecordDelete);
                    _dbContext.SaveChanges();
                    return true;
                }
                _logger.Info("Запись удалена");
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось удалить данные из таблицы WORK в WorkService");
                return false;
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
            work.time_limit = DateTime.SpecifyKind(work.time_limit, DateTimeKind.Utc);
        }
    }
}
