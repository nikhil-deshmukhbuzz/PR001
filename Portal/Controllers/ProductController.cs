using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Microsoft.EntityFrameworkCore;

namespace Portal.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        DB001Core context = new DB001Core();

        public IActionResult Index()
        {
            return View();
        }

        [Route("GetList")]
        [HttpGet]
        public JsonResult GetList(long clientId)
        {

            try
            {
                var output = context.Licenses
                    .Where(c => c.ClientID == clientId &&  c.TotalLicense != 0)
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


        [Route("GetClientTypeList")]
        [HttpGet]
        public JsonResult GetClientTypeList(long productId)
        {

            try
            {
                var output = context.ClientTypes
                    .Where(c => c.ProductID == productId)
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
    }
}