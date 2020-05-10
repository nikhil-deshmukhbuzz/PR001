using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Common;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Distributor")]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] Order _oOrder)
        {

            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    CommonLogic clsCommonLogic = new CommonLogic();
                    _oOrder.OrderNumber = clsCommonLogic.GenerateOrderNumber();
                    _oOrder.OrderDate =  DateTime.Now;
                    _oOrder.OrderStatusID = 1;
                    _oOrder.CreatedOn = DateTime.Now;
                    context.Orders.Add(_oOrder);
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
        public bool Update([FromBody] Order _oOrder)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.Orders
                                   .Where(w => w.OrderID == _oOrder.OrderID)
                                   .FirstOrDefault();

                    input.ClientTypeID = _oOrder.ClientTypeID;
                    input.DeadlineDate = _oOrder.DeadlineDate;
                    input.ModifiedOn = DateTime.Now;
                    input.ModifiedBy = _oOrder.ModifiedBy;

                    if (_oOrder.OrderStatusID != 0)
                        input.OrderStatusID = _oOrder.OrderStatusID;

                    output = context.SaveChanges();

                    //Cancel Order
                    if (_oOrder.OrderStatusID == 6)
                    {
                        var dispatch = context.Dispatchs
                                  .Where(w => w.OrderID == _oOrder.OrderID)
                                  .FirstOrDefault();

                        if (dispatch != null)
                        {
                            dispatch.IsDeleted = true;
                            var update = context.SaveChanges();
                        }

                        var invoice = context.Invoices
                                 .Where(w => w.OrderID == _oOrder.OrderID)
                                 .FirstOrDefault();

                        if (invoice != null)
                        {
                            invoice.IsDeleted = true;
                           var update = context.SaveChanges();
                        }
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
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetList()
        {

            try
            {
                var output = context.Orders
                    .Include(i => i.ClientMaster)
                    .Include(i => i.ProductMaster)
                    .Include(i => i.OrderStatus)
                    .Include(i => i.ClientType)
                    .OrderBy(o => o.OrderStatusID)
                    .ThenByDescending(o => o.CreatedOn)
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

        [Route("GetListByDistributor")]
        [HttpGet]
        public JsonResult GetListByDistributor(long distributorId)
        {

            try
            {
                var output = context.Orders
                    .Include(i => i.ClientMaster)
                    .Include(i => i.ProductMaster)
                    .Include(i => i.OrderStatus)
                    .Include(i => i.ClientType)
                    .Where(w => w.ClientMaster.DistributorID == distributorId)
                    .OrderBy(o => o.OrderStatusID)
                    .ThenByDescending(o => o.CreatedOn)
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
        public JsonResult Get(long orderId)
        {

            try
            {
                var output = context.Orders
                                     .Where(s => s.OrderID == orderId)
                                     .Include(o => o.OrderStatus)
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

        [Route("GetOrderStatusList")]
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetOrderStatusList()
        {

            try
            {
                var output = context.OrderStatuss.ToList();
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