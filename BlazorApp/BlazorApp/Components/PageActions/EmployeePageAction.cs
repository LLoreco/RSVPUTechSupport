using BlazorApp.Components.Data;
using BlazorApp.Components.Model;
using BlazorApp.Components.Services;
using NLog;

namespace BlazorApp.Components.PageActions
{
    public class EmployeePageAction
    {
        private readonly EmployeeMapper _employeeMapper;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public EmployeePageAction(EmployeeMapper employeeMapper)
        {
            _employeeMapper = employeeMapper;
        }
        public List<EmployeeDataModel> OnInitializedEmployeePageAction(EmployeeService service, List<EmployeeDataModel> employeeList)
        {
            try
            {
                var employees = service.GetAllEmployee();
                employeeList = _employeeMapper.MapEmployeesToDataModels(employees);
                return employeeList;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось инициализировать таблицу EMPLOYEE в EmployeePageAction");
                return new List<EmployeeDataModel>();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Ошибка аргумента при инициализации таблицы EMPLOYEE в EmployeePageAction");
                return new List<EmployeeDataModel>();
            }
        }
        public List<EmployeeDataModel> InsertRecordEmployeePageAction(Employee employeeObject, EmployeeService service, List<EmployeeDataModel> employeeDataList, IList<Employee> employeeList)
        {
            try
            {
                var updateEmployeeDetails = false;
                if (employeeObject.id > 0)
                {
                    updateEmployeeDetails = service.UpdateRecord(employeeObject);
                }
                else
                {
                    employeeObject.id = 0;
                    service.InsertRecord(employeeObject);
                }
                employeeList = service.GetAllEmployee();
                employeeDataList = _employeeMapper.MapEmployeesToDataModels(employeeList.ToList());
                return employeeDataList;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу EMPLOYEE в EmployeePageAction");
                return new List<EmployeeDataModel>();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Ошибка аргумента при записи таблицы EMPLOYEE в EmployeePageAction");
                return new List<EmployeeDataModel>();
            }
        }
        public Employee GetEmployeeDetailsEmployeePageAction(IList<Employee> employeeList, EmployeeService service, EmployeeDataModel employeeEdit, Employee employeeObject, List<EmployeeDataModel> employeeDataList)
        {
            try
            {
                employeeList = service.GetAllEmployee();
                foreach (var item in employeeList)
                {
                    if (employeeEdit.Id == item.id)
                    {
                        employeeObject = item;
                        employeeList = service.GetAllEmployee();
                        employeeDataList = _employeeMapper.MapEmployeesToDataModels(employeeList.ToList());
                        break;
                    }
                }
                return employeeObject;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы EMPLOYEE в EmployeePageAction");
                return new Employee();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Ошибка аргумента при инициализации таблицы EMPLOYEE в EmployeePageAction");
                return new Employee();
            }
        }
        public List<EmployeeDataModel> DeleteEmployee(EmployeeService service, EmployeeDataModel employeeDelete, IList<Employee> employeeList, List<EmployeeDataModel> employeeDataList)
        {
            try
            {
                var res = service.DeleteRecord(employeeDelete);
                employeeList = service.GetAllEmployee();
                employeeList.Clear();
                employeeList = service.GetAllEmployee();
                employeeDataList = _employeeMapper.MapEmployeesToDataModels(employeeList.ToList());
                return employeeDataList;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось удалить данные из EMPLOYEE в EmployeePageAction");
                return new List<EmployeeDataModel>();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Ошибка аргумента при инициализации таблицы EMPLOYEE в EmployeePageAction");
                return new List<EmployeeDataModel>();
            }
        }
    }
}
