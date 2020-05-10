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
    public class CityController : Controller
    {
        DB001Core context = new DB001Core();

        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] CityMaster _oCityMaster)
        {

            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    context.CityMasters.Add(_oCityMaster);
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
        public bool Update([FromBody] CityMaster _oCityMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.CityMasters
                                   .Where(w => w.CityID == _oCityMaster.CityID)
                                   .FirstOrDefault();

                    input.CityName = _oCityMaster.CityName;

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
        public JsonResult GetList(long districtId)
        {
            try
            {
                var output = context.CityMasters
                            .Where(s => s.DistrictID == districtId)
                            .Include(i => i.DistrictMaster)
                            .Include(i => i.StateMaster)
                            .OrderBy(o => o.CityName)
                            .ThenBy(o => o.DistrictMaster.DistrictName)
                            .ThenBy(o => o.StateMaster.StateName)
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

        [Route("GetAllList")]
        [HttpGet]
        public JsonResult GetAllList()
        {
            try
            {
                var output = context.CityMasters
                    .Include(i => i.DistrictMaster)
                    .Include(i => i.StateMaster)
                    .OrderBy(o => o.CityName)
                    .ThenBy(o => o.DistrictMaster.DistrictName)
                    .ThenBy(o => o.StateMaster.StateName)
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
        public JsonResult Get(long cityId)
        {

            try
            {
                var output = context.CityMasters
                                     .Where(w => w.CityID == cityId)
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