using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Models;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Inventory Supervisor")]
    [Route("[controller]")]
    public class SpareController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] SpareMaster _oSpareMaster)
        {

            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    context.SpareMasters.Add(_oSpareMaster);
                    output = context.SaveChanges();
                }
                return Convert.ToBoolean(output);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
            }
        }

        [Route("Update")]
        [HttpPost]
        public bool Update([FromBody] SpareMaster _oSpareMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.SpareMasters
                                   .Where(s => s.SpareID == _oSpareMaster.SpareID)
                                   .FirstOrDefault();
                    //  var input = context.SpareMasters.First<SpareMaster>();

                    input.SpareName = _oSpareMaster.SpareName;
                    input.Stock = _oSpareMaster.Stock;

                    output = context.SaveChanges();
                }
                return Convert.ToBoolean(output);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
            }
        }

        [Route("GetList")]
        [HttpGet]
        public JsonResult GetList()
        {

            try
            {
                var output = context.SpareMasters.ToList();
                return Json(output);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
            }
        }

        [Route("Get")]
        [HttpGet]
        public JsonResult Get(long spareId)
        {

            try
            {
                var output = context.SpareMasters
                                     .Where(s => s.SpareID == spareId)
                                     .FirstOrDefault();
                return Json(output);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
            }
        }
    }
}