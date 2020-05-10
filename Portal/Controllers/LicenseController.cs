using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    [Route("[controller]")]
    public class LicenseController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] List<License> listOfLicense)
        {

            try
            {
                int output = 0;

                foreach (var item in listOfLicense)
                {
                    using (var context = new DB001Core())
                    {
                        context.Licenses.Add(item);
                        output = context.SaveChanges();
                    }
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
        public bool Update([FromBody] List<License> listOfLicense)
        {
            try
            {
                    int output = 0;
                
                    foreach (var item in listOfLicense)
                    {
                        using (var context = new DB001Core())
                        {
                            var input = context.Licenses
                                   .Where(s => s.LicenseID == item.LicenseID)
                                   .FirstOrDefault();

                            input.TotalLicense = item.TotalLicense;
                            input.LicenseDueDate = item.LicenseDueDate;

                            output = context.SaveChanges();
                        }
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
                var output = context.Licenses
                    .Include(c => c.ClientMaster)
                    .Include(p => p.ProductMaster)
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
        public JsonResult Get(long licenseId)
        {

            try
            {
                var output = context.Licenses
                                     .Include(p => p.ProductMaster)
                                     .Where(s => s.LicenseID == licenseId)
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

        [Route("GetProductList")]
        [HttpGet]
        public JsonResult GetLGetProductListist(long clientId)
        {

            try
            {
                var license = context.Licenses
                    .Where(s => s.ClientID == clientId)
                    .ToList();

                var output = new List<ProductMaster>();

                if (license.Count == 0)
                {
                    output = context.ProductMasters.ToList();
                }
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