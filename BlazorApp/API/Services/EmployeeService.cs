using NLog;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
namespace API.Services
{
    public class EmployeeService: IEmployeeService
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public EmployeeService(ApplicationDbContext _db)
        {
            _dbContext = _db;
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            try
            {
                return await _dbContext.employees.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы EMPLOYEE в EmployeeService");
                return new List<Employee>();
            }
        }
        public async Task<TaskResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _dbContext.employees.FindAsync(id);
                return new TaskResult<Employee>
                {
                    IsSuccess = true,
                    Result = employee
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы EMPLOYEE в EmployeeService");
                return new TaskResult<Employee>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }

        public async Task<TaskResult<bool>> InsertRecord(Employee employee)
        {
            try
            {
                _dbContext.employees.Add(employee);
                await _dbContext.SaveChangesAsync();
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка добавления записи в EmployeeService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }

        public async Task<TaskResult<Employee>> EditRecord(int employeeID)
        {
            try
            {
                var employee = await _dbContext.employees.FindAsync(employeeID);
                return new TaskResult<Employee>
                {
                    IsSuccess = true,
                    Result = employee
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка редактирования записи в EmployeeService");
                return new TaskResult<Employee>
                {
                    IsSuccess = false,
                    Result = null
                };
            }

        }

        public async Task<TaskResult<bool>> UpdateRecord(Employee employeeUpdate)
        {
            try
            {
                var employeeRecordUpdate = await _dbContext.employees.FindAsync(employeeUpdate.id);
                if (employeeRecordUpdate != null)
                {
                    employeeRecordUpdate.first_name = employeeUpdate.first_name;
                    employeeRecordUpdate.last_name = employeeUpdate.last_name;
                    employeeRecordUpdate.middle_name = employeeUpdate.middle_name;
                    employeeRecordUpdate.division = employeeUpdate.division;
                    employeeRecordUpdate.role = employeeUpdate.role;
                    employeeRecordUpdate.email = employeeUpdate.email;
                    employeeRecordUpdate.phone = employeeUpdate.phone;
                    employeeRecordUpdate.work_amount = employeeUpdate.work_amount;
                    employeeRecordUpdate.salary = employeeUpdate.salary;
                    employeeRecordUpdate.status = employeeUpdate.status;
                    employeeRecordUpdate.password = employeeUpdate.password;
                    employeeRecordUpdate.login = employeeUpdate.login;
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
                _logger.Error(ex, "Ошибка обновления записи в EmployeeService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }

        }
        public async Task<TaskResult<bool>> DeleteRecord(Employee employeeDelete)
        {
            try
            {
                var employeeRecordDelete = await _dbContext.employees.FindAsync(employeeDelete.id);
                if (employeeRecordDelete != null)
                {
                    _dbContext.Remove(employeeRecordDelete);
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
                _logger.Error(ex, "Ошибка удаления записи в EmployeeService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
    }
}
