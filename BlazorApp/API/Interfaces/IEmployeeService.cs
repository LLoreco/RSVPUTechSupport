using API.Data;

namespace API.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployee();
        public Employee GetEmployee(int id);
        public bool InsertRecord(Employee employee);
        public Employee EditRecord(int employeeID);
        public bool UpdateRecord(Employee employeeUpdate);
        public bool DeleteRecord(Employee employeeDelete);
    }
}
