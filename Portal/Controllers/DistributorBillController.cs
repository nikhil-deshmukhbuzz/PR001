using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Common;
using Portal.Models;
using Microsoft.EntityFrameworkCore;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,AccountManger,Distributor")]
    [Route("[controller]")]
    public class DistributorBillController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Update")]
        [HttpPost]
        public bool Update([FromBody] DistributorBill _oDistributorBill)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.DistributorBills
                                   .Where(s => s.DistributorBillID == _oDistributorBill.DistributorBillID)
                                   .FirstOrDefault();

                    input.PaymentModeID = _oDistributorBill.PaymentModeID;
                    input.PaymentStatusID = _oDistributorBill.PaymentStatusID;
                    input.ModifiedOn = DateTime.Now;

                    if (_oDistributorBill.PaymentStatusID == 1 && input.PaymentDate == null)
                    {
                        input.PaymentDate = DateTime.Now;
                    }

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
                var output = context.DistributorBills
                    .Include(i => i.Invoice)
                    .Include(i => i.ClientMaster)
                    .Include(i => i.ProductMaster)
                    .Include(i => i.Order)
                    .Include(i => i.Distributor)
                    .Include(i => i.PaymentStatus)
                    .OrderByDescending(o => o.CreatedOn)
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
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Get(long distributorbillId)
        {
            try
            {
                var output = context.DistributorBills
                                     .Where(w => w.DistributorBillID == distributorbillId)
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

        [Route("FilterByDate")]
        [AllowAnonymous]
        [HttpGet]
        public JsonResult FilterByDate(long distributorId, int month, int year)
        {

            try
            {
                var output = context.DistributorBills
                   .Include(i => i.Invoice)
                   .Include(i => i.ClientMaster)
                   .Include(i => i.ProductMaster)
                   .Include(i => i.Order)
                   .Include(i => i.Distributor)
                   .Include(i => i.PaymentStatus)
                   .OrderByDescending(o => o.CreatedOn)
                   .Where(
                           w => (distributorId == 0 ? true : w.DistributorID == distributorId) &&
                           (month == 0 ? true : w.CreatedOn.Month == month) &&
                           (year == 0 ? true : w.CreatedOn.Year == year)
                   ).ToList();
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