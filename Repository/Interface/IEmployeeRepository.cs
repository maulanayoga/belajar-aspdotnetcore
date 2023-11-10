using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetData();
        IEnumerable<EmpVM> GetDataEmp();
        Employee Get(string NIK);
        int Insert(EmployeeVM employeevm);
        int Update(UpdateVM update);
        int Delete(string NIK);
    }
}
