using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestAPI.Models;
using TestAPI.Repository;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository repository;
        public EmployeeController(EmployeeRepository repository)
        {
            this.repository = repository;
        }
        [EnableCors]
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetData()
        {
            try
            {
                var get = repository.GetData();

                if (get.Count() == 0)
                {

                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }
                else
                {
                    return CreateResponse(HttpStatusCode.OK, "Data Employee", get);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [EnableCors]
        [HttpGet("Emp")]
        public ActionResult<IEnumerable<EmpVM>> GetDataEmp()
        {
            try
            {
                var get = repository.GetDataEmp();

                if (get.Count() == 0)
                {

                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }
                else
                {
                    return CreateResponse(HttpStatusCode.OK, "Data Employee", get);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [EnableCors]
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            try
            {
                var GetNIK = repository.Get(NIK);

                if (GetNIK == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }

                return CreateResponse(HttpStatusCode.OK, "Data Employee", GetNIK);
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [EnableCors]
        [HttpPost]
        public virtual ActionResult Insert(EmployeeVM employeevm)
        {
            try
            {
                //var CekNo = repository.CheckPhone(employeevm.Phone);
                //var CekEmail = repository.CheckEmail(employeevm.Email);

                if (employeevm == null)
                {
                    return CreateResponse(HttpStatusCode.Conflict, "!");
                }
                var insert = repository.Insert(employeevm);

                return CreateResponse(HttpStatusCode.OK, "Data Inserted!", employeevm);
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [EnableCors]
        [HttpDelete("{NIK}")]
        public virtual ActionResult Delete(string NIK)
        {
            try
            {
                var employee = repository.Get(NIK);

                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }
                var delete = repository.Delete(NIK);
                return CreateResponse(HttpStatusCode.OK, "Data Deleted!");

            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [EnableCors]
        [HttpPut]
        public virtual ActionResult Put(UpdateVM update)
        {
            try
            {
  
                //var get = repository.Get(employee);
                if (update == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }

                var upt = repository.Update(update);
                return CreateResponse(HttpStatusCode.OK, "Data Updated!");
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [EnableCors]
        [HttpGet("TestCors")]
        public ActionResult TestCors()
        {
            return Ok("Test CORS");
        }
        private ActionResult CreateResponse(HttpStatusCode statusCode, string message, object data = null)
        {
            if (data == null)
            {
                var responseDataNull = new JsonResult(new
                {
                    status_code = (int)statusCode,
                    message,
                });

                return responseDataNull;

            }

            var response = new JsonResult(new
            {
                status_code = (int)statusCode,
                message,
                data
            });

            return response;
        }


    }
}
