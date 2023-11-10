using TestAPI.ViewModels;

namespace TestAPI.Repository.Interface
{
    public interface IChartRepository
    {
        IEnumerable<ChartVM> GetDepEmp();
    }
}
