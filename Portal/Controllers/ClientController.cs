using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Common;
using Portal.Models;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] ClientMaster _oClientMaster)
        {

            try
            {
                CommonLogic clsCommon = new CommonLogic();
                _oClientMaster.ClientCode = clsCommon.GenerateClientCode(_oClientMaster.ClientName);




                int output;
                using (var context = new DB001Core())
                {
                    context.ClientMasters.Add(_oClientMaster);
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
        public bool Update([FromBody] ClientMaster _oClientMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.ClientMasters
                                   .Where(s => s.ClientID == _oClientMaster.ClientID)
                                   .FirstOrDefault();
                    //  var input = context.ClientMasters.First<ClientMaster>();

                    input.ClientName = _oClientMaster.ClientName;
                    input.ContactPerson = _oClientMaster.ContactPerson;
                    input.Email = _oClientMaster.Email;
                    input.MobileNo = _oClientMaster.MobileNo;
                    input.Address = _oClientMaster.Address;
                    input.IsActive = _oClientMaster.IsActive;
                    input.CityID = _oClientMaster.CityID;
                    input.DistrictID = _oClientMaster.DistrictID;
                    input.StateID = _oClientMaster.StateID;
                    input.DistributorID = _oClientMaster.DistributorID;

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
                var output = context.ClientMasters.ToList();
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
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Get(long clientId)
        {

            try
            {
                var output = context.ClientMasters
                                     .Where(s => s.ClientID == clientId)
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