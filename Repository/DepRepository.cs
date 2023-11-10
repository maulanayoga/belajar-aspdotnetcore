using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.Linq.Dynamic.Core;
using TestAPI.Context;
using TestAPI.Models;
using TestAPI.Repository.Interface;
using TestAPI.ViewModels;

namespace TestAPI.Repository
{
    public class DepRepository : IDepRepository
    {
        private readonly MyContext context;

        public DepRepository(MyContext context)
        {
            this.context = context;
        }

        int counter = 0;
        public string GenDep()
        {
            var LastData = context.Departements.OrderByDescending(e => e.Id).FirstOrDefault();

            string NewDepId;

            if (LastData == null)
            {
                counter = 1;

            }
            else
            {
                string LastId = LastData.Id;
                string CounterData = LastId.Substring(LastId.Length - 3);

                counter = int.Parse(CounterData) + 1;
            }
            string UniqueNumber = counter.ToString().PadLeft(3, '0');
            NewDepId = "D" + UniqueNumber;
            counter++;
            return NewDepId;

        }

        public bool CheckEmp(string Id)
        {
            var emp = context.Employees.FirstOrDefault(e => e.DepId == Id);
            if (emp == null)
            {
                return true;
            }
            return false;
        }

        public int Delete(string Id)
        {
            var entity = context.Departements.Find(Id);
            context.Remove(entity);
            var save = context.SaveChanges();
            return save;
        }

        public DepVM Get(string Id)
        {

            var departement = (from dep in context.Departements
                                      //join emp in context.Employees on dep.Id equals emp.DepId
                                  select new DepVM
                                  {
                                      Id = dep.Id,
                                      Name = dep.Name,
                                  }).SingleOrDefault(d => d.Id == Id);
            return departement;
            
        }

        public IEnumerable<Departement> GetData()
        {
            return context.Departements.ToList();
        }

        public IEnumerable<Departement> GetDepEmp()
        {
            var result = context.Departements.Select(department => new
            {
                DepartmentName = department.Name,
                EmployeeCount = context.Employees.Count(employee => employee.DepId == department.Id)
            }).ToList();

            return (IEnumerable<Departement>)result;
        }

        public int Insert(DepVM depvm)
        {
            var dep = new Departement
            {
                Id = GenDep(),
                Name = depvm.Name,
            };
            context.Departements.Add(dep);
        
            return context.SaveChanges();
        }

        public int Update(DepVM depvm)
        {
            //var departements = context.Departements.FirstOrDefault(d => d.Id == depvm.Id);
            var dep = new Departement
            {
                Id = depvm.Id,
                Name = depvm.Name,
            };
            if (dep.Id == null)
            {
                return 0;
            }
            context.Entry(dep).State = EntityState.Modified;
            context.SaveChanges();
            return 1;
        }

        public Object GetDataSS(DataTableRequest ReqTable)
        {
            int totalRecord = 0;
            int filterRecord = 0;
            var data = context.Departements.AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(ReqTable.searchValue))
            {
                data = data.Where(x => x.Id.ToLower().Contains(ReqTable.searchValue.ToLower())
                                    || x.Name.ToLower().Contains(ReqTable.searchValue.ToLower()));
            }
            // get total count of records after search
            filterRecord = data.Count();
            //sort data
            if (!string.IsNullOrEmpty(ReqTable.sortColumn) && !string.IsNullOrEmpty(ReqTable.sortColumnDirection)) data = data.OrderBy(ReqTable.sortColumn + " " + ReqTable.sortColumnDirection);

            //pagination
            var depList = data.Skip(ReqTable.skip).Take(ReqTable.pageSize).ToList();
            var returnObj = new
            {
                draw = ReqTable.draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = depList
            };
            return returnObj;
        }
    }
}
