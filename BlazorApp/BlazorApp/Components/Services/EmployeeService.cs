using BlazorApp.Components.Data;
using BlazorApp.Components.Interfaces;
using BlazorApp.Components.Model;
using NLog;
using System.Security.Claims;
namespace BlazorApp.Components.Services
{
    public class EmployeeService: IEmployeeService
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public EmployeeService(ApplicationDbContext _db)
        {
            _dbContext = _db;
        }

        public List<Employee> GetAllEmployee()
        {
            try
            {
                var employees = _dbContext.employees.ToList().OrderBy(e => e.id).ToList();
                _logger.Info("Работники получены");
                return employees;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы EMPLOYEE в EmployeeService");
                return new List<Employee>();
            }
        }

        public bool InsertRecord(Employee employee)
        {
            try
            {
                _dbContext.employees.Add(employee);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось ввести данные в таблицу EMPLOYEE в EmployeeService");
                return false;
            }
            
        }

        public Employee EditRecord(int employeeID)
        {
            try
            {
                Employee ec = new Employee();
                return _dbContext.employees.FirstOrDefault(u => u.id == employeeID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось ввести данные в таблицу EMPLOYEE в EmployeeService");
                return new Employee();
            }
            
        }

        public bool UpdateRecord(Employee employeeUpdate)
        {
            try
            {
                var employeeRecordUpdate = _dbContext.employees.FirstOrDefault(u => u.id == employeeUpdate.id);
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
                    _dbContext.SaveChanges();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось обновить данные в таблице EMPLOYEE в EmployeeService");
                return false;
            }
           
        }
        public bool DeleteRecord(EmployeeDataModel employeeDelete)
        {
            try
            {
                var employeeRecordDelete = _dbContext.employees.FirstOrDefault(u => u.id == employeeDelete.Id);
                if (employeeRecordDelete != null)
                {
                    _dbContext.Remove(employeeRecordDelete);
                    _dbContext.SaveChanges();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось удалить запись из таблицы EMPLOYEE в EmployeeService");
                return false;
            }
            
        }
        public Employee FindEmployee(LoginViewModel loginUsername)
        {
            var user= _dbContext.employees.FirstOrDefault(x => x.login == loginUsername.UserName);
            if (user == null || user.password != loginUsername.Password)
            {
                return null;
            }
            else
            {
                return user;
            }
            
        }
    }
}
