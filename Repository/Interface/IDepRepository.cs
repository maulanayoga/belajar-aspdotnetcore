using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Repository.Interface
{
    public interface IDepRepository
    {
        IEnumerable<Departement> GetData();
        DepVM Get(string Id);

        Object GetDataSS(DataTableRequest ReqTable);
        int Insert(DepVM depvm);
        int Update(DepVM dep);
        int Delete(string Id);

        IEnumerable<Departement> GetDepEmp();
    }
}
