using API.Data;
using API.Interfaces;
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
                var item = _dbContext.work.OrderBy(e => e.id).ToList();
                return item;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы WORK в WorkService");
                return new List<Work>();
            }
        }
        public Work GetWork(int id)
        {
            try
            {
                return _dbContext.work.Find(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы WORK в WorkService");
                return new Work();
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

        public bool UpdateRecord(Work workUpdate)
        {
            try
            {
                var workRecordUpdate = _dbContext.work.Include(w => w.Employee).FirstOrDefault(u => u.id == workUpdate.id);
                if (workRecordUpdate != null)
                {
                    workRecordUpdate.work_number = workUpdate.work_number;
                    workRecordUpdate.description = workUpdate.description;
                    workRecordUpdate.send_time = workUpdate.send_time.ToUniversalTime();
                    workRecordUpdate.time_limit = workUpdate.time_limit.ToUniversalTime();
                    workRecordUpdate.total_time = workUpdate.total_time.ToUniversalTime();
                    workRecordUpdate.status = workUpdate.status;

                    var availableEmployee = _dbContext.employees.FirstOrDefault(e => e.id == workUpdate.employee_id);
                    if (availableEmployee != null)
                    {
                        workRecordUpdate.employee_id = availableEmployee.id;
                        workRecordUpdate.image = FindEmployee(workRecordUpdate.employee_id);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        workUpdate.employee_id = workRecordUpdate.employee_id;
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

        public bool DeleteRecord(Work workDelete)
        {
            try
            {
                var workRecordDelete = _dbContext.work.FirstOrDefault(u => u.id == workDelete.id);
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
