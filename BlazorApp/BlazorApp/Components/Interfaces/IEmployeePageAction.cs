using BlazorApp.Components.Data;
using BlazorApp.Components.Model;
using BlazorApp.Components.Services;

namespace BlazorApp.Components.Interfaces
{
    public interface IEmployeePageAction
    {
        List<EmployeeDataModel> OnInitializedEmployeePageAction(EmployeeService service, List<EmployeeDataModel> employeeList);
        List<EmployeeDataModel> InsertRecordEmployeePageAction(Employee employeeObject, EmployeeService service, List<EmployeeDataModel> workDataList, IList<Employee> employeeList);
        Employee GetEmployeeDetailsEmployeePageAction(IList<Employee> employeeList, EmployeeService service, EmployeeDataModel employeeEdit, Employee employeeObject, List<EmployeeDataModel> employeeDataList);
        List<EmployeeDataModel> DeleteEmployee(EmployeeService service, EmployeeDataModel employeeDelete, IList<Employee> employeeList, List<EmployeeDataModel> employeeDataList);
    }
}
