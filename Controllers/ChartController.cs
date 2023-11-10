using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestAPI.Context;
using TestAPI.Models;
using TestAPI.Repository;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly ChartRepository repository;
        private readonly MyContext context;
        public ChartController(ChartRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ChartVM>> GetDepEmp()
        {
            try
            {
                var get = repository.GetDepEmp();

                if (get.Count() == 0)
                {

                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }
                else
                {
                    return CreateResponse(HttpStatusCode.OK, "Data Departement", get);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
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
