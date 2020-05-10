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
    [Authorize(Roles = "Superadmin,Administrator")]
    [Route("[controller]")]
    public class StateController : Controller
    {
        DB001Core context = new DB001Core();

        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] StateMaster _oStateMaster)
        {

            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    context.StateMasters.Add(_oStateMaster);
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
        public bool Update([FromBody] StateMaster _oStateMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.StateMasters
                                   .Where(s => s.StateID == _oStateMaster.StateID)
                                   .FirstOrDefault();

                    input.StateName = _oStateMaster.StateName;

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
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetList()
        {

            try
            {
                var output = context.StateMasters
                    .OrderBy(o => o.StateName)
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
        public JsonResult Get(long stateId)
        {

            try
            {
                var output = context.StateMasters
                                     .Where(w => w.StateID == stateId)
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