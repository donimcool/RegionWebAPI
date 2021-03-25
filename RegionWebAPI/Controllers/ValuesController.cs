using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RegionWebAPI.Data;
using RegionWebAPI.Models;

namespace RegionWebAPI.Controllers
{

    [RoutePrefix("api")]
    public class ValuesController : ApiController
    {
        public DataLayer Data { get; set; } = new DataLayer();

        // GET api/values/5
        [Route("region/{id}/employees")]
        public async Task<HttpResponseMessage> Get([FromUri] int id)
        {
            try
            {
                BaseResponse b = new BaseResponse(false, "");
                var emps = Data.EmployeeRepo().Where(e => e.RegionId == id).ToList();
                if (emps != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { emps, b });
                }
                else
                {
                    b = new BaseResponse(false, "Employee Not Found");
                    return Request.CreateResponse(HttpStatusCode.OK, b);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }

        [Route("employee")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Employee employee)
        {
            try
            {
               if (Data.RegionRepo().Any(x => x.RegionId == employee.RegionId))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No such region");
                }
                var res = Data.SaveEmployee(employee);

                if (res.Item1)
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, res.Item2);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }

        [Route("region")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Region region)
        {
            try
            {
              var res =  Data.SaveRegion(region);
                if(res.Item1)
                     return Request.CreateResponse(HttpStatusCode.OK, "Success");
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, res.Item2);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

    }
}
