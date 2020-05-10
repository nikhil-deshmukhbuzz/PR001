using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Models;
using Microsoft.EntityFrameworkCore;

namespace Portal.Controllers
{
    [Route("[controller]")]
    public class DistrictController : Controller
    {
        DB001Core context = new DB001Core();

        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] DistrictMaster _oDistrictMaster)
        {

            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    context.DistrictMasters.Add(_oDistrictMaster);
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
        public bool Update([FromBody] DistrictMaster _oDistrictMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.DistrictMasters
                                   .Where(w => w.DistrictID == _oDistrictMaster.DistrictID)
                                   .FirstOrDefault();

                    input.DistrictName = _oDistrictMaster.DistrictName;

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
                var output = context.DistrictMasters
                            .Include(i => i.StateMaster)
                            .OrderBy(o => o.DistrictName)
                            .ToList();
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

        [Route("GetStateWiseList")]
        [HttpGet]
        public JsonResult GetStateWiseList(long stateId)
        {
            try
            {
                var output = context.DistrictMasters
                                     .Where(s => s.StateID == stateId)
                                     .OrderBy(o => o.DistrictName)
                                     .ToList();
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
        public JsonResult Get(long districtId)
        {

            try
            {
                var output = context.DistrictMasters
                                     .Where(w => w.DistrictID == districtId)
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