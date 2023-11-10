using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using TestAPI.Context;
using TestAPI.Models;
using TestAPI.Repository;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepController : ControllerBase
    {
        private readonly DepRepository repository;
        private readonly MyContext context;

        public DepController(DepRepository repository, MyContext context)
        {
            this.repository = repository;
            this.context = context;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Departement>> GetData()
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
                    return CreateResponse(HttpStatusCode.OK, "Data Departement", get);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        //[HttpPost("paging")]
        //public IActionResult GetDataSS([FromBody] DataTableRequest request)
        //{
        //    try
        //    {
        //        var query = repository.GetData().AsQueryable(); // Query untuk Departement

        //        // Terapkan pencarian jika ada
        //        if (!string.IsNullOrEmpty(request.Search?.Value))
        //        {
        //            query = query.Where(d => d.Id.ToLower().Contains(request.Search.Value) || d.Name.Contains(request.Search.Value));

        //        }

        //        // Hitung total rekaman sebelum filtering
        //        var totalRecords = query.Count();

        //        // Terapkan pembagian halaman
        //        var departments = query
        //            .Skip(request.Start)
        //            .Take(request.Length)
        //            .ToList();

        //        // Format respons
        //        var response = new
        //        {
        //            draw = request.Draw,
        //            recordsTotal = totalRecords, // Total rekaman dalam basis data sebelum filtering
        //            recordsFiltered = totalRecords, // Total rekaman setelah filtering (tidak ada pencarian berbasis server dalam contoh ini)
        //            data = departments // Data yang akan ditampilkan di tabel
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //}

        [HttpPost("page")]
        public ActionResult GetPaging()
        {
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;
                var draw = Request.Form["draw"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                
                var ReqTable = new DataTableRequest
                {
                    draw = draw,
                    sortColumn = sortColumn,
                    sortColumnDirection = sortColumnDirection,
                    searchValue = searchValue,
                    pageSize = pageSize,
                    skip = skip
                };

                var data = repository.GetDataSS(ReqTable);

                return Ok(data);

            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            

        }


        [HttpGet("{Id}")]
        public ActionResult Get(string Id)
        {
            try
            {
                var GetId = repository.Get(Id);

                if (GetId == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }

                return CreateResponse(HttpStatusCode.OK, "Data Departement", GetId);
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost("insert")]
        public virtual ActionResult Insert(DepVM depvm)
        {
            try
            {
                var dep = repository.Insert(depvm);
                if (depvm.Name == "")
                {
                    return CreateResponse(HttpStatusCode.Conflict, "Data null!");
                }

                return CreateResponse(HttpStatusCode.OK, "Data Inserted!", dep);
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public virtual ActionResult Delete(string Id)
        {
            try
            {
                var dep = repository.Get(Id);
                var emp = repository.CheckEmp(Id);
                if (dep == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not Exsist!");
                }else if(emp == false)
                {
                    return CreateResponse(HttpStatusCode.Conflict, "Data Cannot Be Deleted!");
                }
                var delete = repository.Delete(Id);
                return CreateResponse(HttpStatusCode.OK, "Data Deleted!");

            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public virtual ActionResult Put(DepVM dep)
        {
            try
            {
                var upt = repository.Update(dep);
                if (upt == null)
                {
                    return CreateResponse(HttpStatusCode.BadRequest, "Data Null!");
                }
                return CreateResponse(HttpStatusCode.OK, "Data Updated!");
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


//int totalRecord = 0;
//int filterRecord = 0;
//var draw = Request.Form["draw"].FirstOrDefault();
//var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
//var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
//var searchValue = Request.Form["search[value]"].FirstOrDefault();
//int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
//int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
//var data = context.Set<Departement>().AsQueryable();
////get total count of data in table
//totalRecord = data.Count();
//// search data when search value found
//if (!string.IsNullOrEmpty(searchValue))
//{
//    data = data.Where(x => x.Id.ToLower().Contains(searchValue.ToLower())
//                        || x.Name.ToLower().Contains(searchValue.ToLower()));
//}
//// get total count of records after search
//filterRecord = data.Count();
////sort data
//if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) data = data.OrderBy(sortColumn + " " + sortColumnDirection);
////pagination
//var empList = data.Skip(skip).Take(pageSize).ToList();
//var returnObj = new
//{
//    draw = draw,
//    recordsTotal = totalRecord,
//    recordsFiltered = filterRecord,
//    data = empList
//};
