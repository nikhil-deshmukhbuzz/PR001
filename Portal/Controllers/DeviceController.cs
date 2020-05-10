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
    public class DeviceController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] DeviceMaster _oDeviceMaster)
        {

            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    context.DeviceMasters.Add(_oDeviceMaster);
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
        public bool Update([FromBody] DeviceMaster _oDeviceMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.DeviceMasters
                                   .Where(s => s.DeviceID == _oDeviceMaster.DeviceID)
                                   .FirstOrDefault();

                    input.DeviceName = _oDeviceMaster.DeviceName;
                    input.Stock = _oDeviceMaster.Stock;
                    
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
                var output = context.DeviceMasters.ToList();
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
        public JsonResult Get(long deviceId)
        {

            try
            {
                var output = context.DeviceMasters
                                     .Where(s => s.DeviceID == deviceId)
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