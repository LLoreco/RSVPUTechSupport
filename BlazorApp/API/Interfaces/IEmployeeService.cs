using API.Data;
using API.Models;

namespace API.Interfaces
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> GetAllEmployee();
        public Task<TaskResult<Employee>> GetEmployee(int id);
        public Task<TaskResult<bool>> InsertRecord(Employee employee);
        public Task<TaskResult<Employee>> EditRecord(int employeeID);
        public Task<TaskResult<bool>> UpdateRecord(Employee employeeUpdate);
        public Task<TaskResult<bool>> DeleteRecord(Employee employeeDelete);
    }
}
