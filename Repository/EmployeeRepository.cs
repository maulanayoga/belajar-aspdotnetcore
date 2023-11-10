using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using TestAPI.Context;
using TestAPI.Models;

using TestAPI.Repository.Interface;
using TestAPI.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TestAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository

    {
        private readonly MyContext context;

        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int counter = 0;

        public string GenNIK()
        {
            var LastData = context.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();

            string NewNIK;

            if (LastData == null)
            {
                counter = 0;

            }
            else
            {
                string LastNIK = LastData.NIK;
                string CounterData = LastNIK.Substring(LastNIK.Length - 3);

                counter = int.Parse(CounterData) + 1;
            }
            string UniqueNumber = counter.ToString().PadLeft(3, '0');
            NewNIK = DateTime.Now.ToString("ddMMyy") + UniqueNumber;
            counter++;
            return NewNIK;
        }

        public string GenEmail(string FirstName, string LastName)
        {
            var LastData = context.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();

            string NewEmail;

            if (LastData == null)
            {
                counter = 0;

            }
            else
            {
                string LastNIK = LastData.NIK;
                string CounterData = LastNIK.Substring(LastNIK.Length - 3);

                counter = int.Parse(CounterData) + 1;
            }
            string UniqueNumber = counter.ToString().PadLeft(3, '0');
            //FirstName = LastData.FirstName;
            //LastName = LastData.LastName;
            NewEmail = $"{FirstName.ToLower()}.{LastName.ToLower()}@berca.co.id";
            counter++;
            return NewEmail;

        }

        private string GeneratedEmail(string FirstName, string LastName)
        {
            string baseEmail = $"{FirstName.ToLower()}.{LastName.ToLower()}";
            string generatedEmail = $"{baseEmail}{"@berca.co.id"}";
            int counter = 1;

            // Check if the generated username already exists in the database
            while (context.Employees.Any(u => u.Email == generatedEmail))
            {
                generatedEmail = $"{baseEmail}{counter:D3}{"@berca.co.id"}"; // Append a three-digit number
                counter++;

                // To avoid infinite loops, you can add a maximum number of retries here.
                if (counter > 999)
                {
                    throw new Exception("Unable to generate a unique username.");
                }
            }

            return generatedEmail;
        }

        public bool CheckNIK(String NIK)
        {
            var C_NIK = context.Employees.AsNoTracking().FirstOrDefault(employee => NIK == employee.NIK);

            if (C_NIK != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckPhone(String Phone)
        {
            var Dupiclate = context.Employees.AsNoTracking().FirstOrDefault(employee => Phone == employee.Phone);

            if (Dupiclate != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckEmail(String Email)
        {
            var Dupiclate = context.Employees.AsNoTracking().FirstOrDefault(employee => Email == employee.Email);

            if (Dupiclate != null)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Employee> GetData()
        {
            return context.Employees.ToList();
        }

        public IEnumerable<EmpVM> GetDataEmp()
        {
            var employee = (from emp in context.Employees
                            join dep in context.Departements on emp.DepId equals dep.Id
                            select new EmpVM
                            {
                                NIK = emp.NIK,
                                FullName = emp.FirstName + " " + emp.LastName,
                                Email = emp.Email,
                                Phone = emp.Phone,
                                Address = emp.Address,
                                DepName = dep.Name,
                                Status = emp.Status,
                            });

            return employee.ToList();
        }

        public Employee Get(string NIK)
        {
            return context.Employees.Find(NIK);
        }

        public int Insert(EmployeeVM employeevm)
        {
            var employee = new Employee
            {
                NIK = GenNIK(),
                FirstName = employeevm.FirstName,
                LastName = employeevm.LastName,
                Email = GeneratedEmail(employeevm.FirstName, employeevm.LastName),//employeevm.FirstName + "." + employeevm.LastName + "@berca.co.id",
                Phone = employeevm.Phone,
                Address = employeevm.Address,
                Status = (Models.Status)employeevm.Status,
                DepId = employeevm.DepId,
            };
            context.Employees.Add(employee);

            var result = context.SaveChanges();
            return result;
        }

        public int Update(UpdateVM update)
        {
            var emp = context.Employees.FirstOrDefault(emp => emp.NIK == update.NIK);
            //   var emp = new Employee
            //   {
            //       NIK = update.NIK;
            //       emp.FirstName = update.FirstName;
            //       emp.LastName = update.LastName;
            //       emp.Phone = update.Phone;
            //       emp.Email = update.Email;
            //       emp.Address = update.Address;
            //       emp.Status = (Models.Status)update.Status;
            //       emp.DepId = update.DepId;

            //};
            emp.NIK = update.NIK;
            emp.FirstName = update.FirstName;
            emp.LastName = update.LastName;
            emp.Phone = update.Phone;
            emp.Email = update.Email;
            emp.Address = update.Address;
            emp.Status = (Models.Status)update.Status;
            emp.DepId = update.DepId;
            context.Entry(emp).State = EntityState.Modified;
            var save = context.SaveChanges();
            return save;
     }
        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            context.Remove(entity);
            var save = context.SaveChanges();
            return save;
        }
    }

}
