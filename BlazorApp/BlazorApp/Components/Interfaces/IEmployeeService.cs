using BlazorApp.Components.Data;
using BlazorApp.Components.Model;

namespace BlazorApp.Components.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployee();
        public bool InsertRecord(Employee employee);
        public Employee EditRecord(int employeeID);
        public bool UpdateRecord(Employee employeeUpdate);
        public bool DeleteRecord(EmployeeDataModel employeeDelete);
    }
}
