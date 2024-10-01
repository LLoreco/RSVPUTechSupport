using BlazorApp.Components.Data;
using BlazorApp.Components.Model;

namespace BlazorApp.Components.PageActions
{
    public class EmployeeMapper
    {
        public EmployeeDataModel MapEmployeeToDataModel(Employee employee)
        {
            return new EmployeeDataModel
            {
                Id = employee.id,
                fullName = $"{employee.last_name} {employee.first_name} {employee.middle_name}",
                division = employee.division,
                role = employee.role,
                phone = employee.phone,
                email = employee.email,
                work_amount = employee.work_amount,
                salary = employee.salary,
                status = employee.status,
                password = employee.password,
                login = employee.login,
            };
        }
        public List<EmployeeDataModel> MapEmployeesToDataModels(List<Employee> employees)
        {
            return employees.Select(MapEmployeeToDataModel).ToList();
        }
    }
}
