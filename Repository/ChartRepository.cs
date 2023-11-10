using TestAPI.Context;
using TestAPI.ViewModels;

namespace TestAPI.Repository
{
    public class ChartRepository
    {
        private readonly MyContext context;
        public ChartRepository(MyContext context)
        {
            this.context = context;
        }

        public IEnumerable<ChartVM> GetDepEmp()
        {
            var query = context.Employees
                .GroupBy(employee => new { employee.Departement.Name })
                .Select(group => new ChartVM
                {
                    departmentName = group.Key.Name,
                    statusActive = group.Count(employee => (Models.Status)employee.Status == 0),
                    employeeCount = group.Count(),
                   
                }).ToList();
            return query;
        }
    }

}

