using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using Portal.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Inventory Supervisor")]
    [Route("[controller]")]
    public class DispatchController : Controller
    {
        
        DB001Core context = new DB001Core();

        private readonly IHostingEnvironment _hostingEnvironment;

        public DispatchController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] Dispatch _oDispatch)
        {

            try
            {
                int output;
                
                using (var context = new DB001Core())
                {
                    CommonLogic clsCommonLogic = new CommonLogic();
                    _oDispatch.DispatchNumber = clsCommonLogic.GenerateDispatchCode();

                    _oDispatch.CreatedOn = DateTime.Now;
                    _oDispatch.ModifiedOn = DateTime.Now;
                    _oDispatch.IsDeleted = false;

                    context.Dispatchs.Add(_oDispatch);
                    output = context.SaveChanges();
                }

                CommonLogic commonLogic = new CommonLogic();
                commonLogic.ChangeOrderStatus(_oDispatch.OrderID, "Device Ready");

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
        public bool Update([FromBody] Dispatch _oDispatch)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.Dispatchs
                                   .Include(d => d.DispatchDetails)
                                   .Where(s => s.DispatchID == _oDispatch.DispatchID)
                                   .FirstOrDefault();

                    input.ClientID = _oDispatch.ClientID;
                    input.ProductID = _oDispatch.ProductID;
                    input.StateID = _oDispatch.StateID;
                    input.DistrictID = _oDispatch.DistrictID;
                    input.CityID = _oDispatch.CityID;
                    input.ShippingAddress = _oDispatch.ShippingAddress;
                    input.DispatchDate = _oDispatch.DispatchDate;
                    input.DispatchDetails = _oDispatch.DispatchDetails;
                    input.IsDispatched = _oDispatch.IsDispatched;
                    input.ModifiedOn = DateTime.Now;

                    if (_oDispatch.IsDispatched == true)
                    {
                        if(input.DispatchDate == null)
                        {
                            input.DispatchDate = DateTime.Now;
                        }
                    }
                        output = context.SaveChanges();
                }

                if (_oDispatch.IsDispatched == true)
                {
                    var invoice = context.Invoices
                        .Where(w => w.OrderID == _oDispatch.OrderID && w.PaymentStatus.Status == "Pending")
                        .FirstOrDefault();

                    if(invoice != null)
                    {
                        invoice.PaymentStatusID = 2; //Unpaid
                        context.SaveChanges();
                    }

                    CommonLogic commonLogic = new CommonLogic();
                    commonLogic.ChangeOrderStatus(_oDispatch.OrderID, "Delivered");
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
                var output = context.Dispatchs
                    .Include(i => i.ClientMaster)
                    .Include(i => i.ProductMaster)
                    .Include(i => i.Order)
                    .Include(i => i.StateMaster)
                    .Include(i => i.DistrictMaster)
                    .Include(i => i.CityMaster)
                    .Where(w => w.IsDeleted == false)
                    .OrderBy(o =>  o.IsDispatched)
                    .ThenByDescending(o => o.DispatchDate)
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
        [HttpGet]
        public JsonResult Get(long dispatchId)
        {

            try
            {
                var output = context.Dispatchs
                                     .Where(s => s.DispatchID == dispatchId)
                                     .FirstOrDefault();

                output.DispatchDetails = context.DispatchDetails
                     .Where(s => s.DispatchID == dispatchId)
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

        [Route("GetInventoryList")]
        [HttpGet]
        public JsonResult GetInventoryList(bool isEditable)
        {
            try
            {
                if (isEditable == false)
                {
                    var output = context.InventoryMasters
                        .FromSql("Select * from InventoryMasters i where i.InventoryID not in (select InventoryID from DispatchDetails dd inner join Dispatchs d on dd.DispatchID = d.DispatchID where d.IsDeleted = 0)")
                        .Include(d => d.DeviceMaster)
                        .Include(d => d.SpareMaster)
                        .ToList();
                    return Json(output);
                }
                else
                {
                    var output = context.InventoryMasters
                        .Include(d => d.DeviceMaster)
                        .Include(d => d.SpareMaster)
                        .ToList();


                    return Json(output);
                }
                
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

        [Route("GetOrderList")]
        [HttpGet]
        public JsonResult GetOrderList(long clientId,long productId)
        {
            try
            {

                    var output = context.Orders
                        .Where(w => w.ClientID == clientId && w.ProductID == productId && w.OrderStatus.Status != "Cancel")
                        .Include(o => o.OrderStatus)
                        .Where(w => w.OrderStatus.Status != "Cancel")
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

        [Route("Delete")]
        [HttpPost]
        public bool Delete([FromBody] Dispatch _oDispatch)
        {
            try
            {
                int output;

                using (var context = new DB001Core())
                {

                    var input = context.Orders
                       .Where(o => o.OrderID == _oDispatch.OrderID)
                       .FirstOrDefault();

                    input.OrderStatusID = 1; //Pending

                    var status = context.SaveChanges();
                }

               
                using (var context = new DB001Core())
                {
                    var input = context.Dispatchs
                                   .Where(w => w.DispatchID == _oDispatch.DispatchID)
                                   .FirstOrDefault();

                    input.IsDeleted = true;
                   
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

        [Route("InvoiceAvailable")]
        [HttpGet]
        public JsonResult InvoiceAvailable(long orderId)
        {
            try
            {
                bool invoiceAvailable = false;
                var output = context.Invoices
                                     .Where(w => w.OrderID == orderId && w.IsDraft == false)
                                     .FirstOrDefault();

                if(output != null)
                {
                    invoiceAvailable = true;
                }
                else
                {
                    invoiceAvailable = false;
                }

                return Json(invoiceAvailable);
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

        [Route("DraftAvailable")]
        [HttpGet]
        public JsonResult DraftAvailable(long orderId)
        {
            try
            {
                bool draftAvailable = false;
                var output = context.Invoices
                                     .Where(w => w.OrderID == orderId)
                                     .FirstOrDefault();

                if (output != null)
                {
                    draftAvailable = true;
                }
                else
                {
                    draftAvailable = false;
                }

                return Json(draftAvailable);
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

        [Route("MakeInvoice")]
        [HttpPost]
        public JsonResult MakeInvoice([FromBody] Dispatch _oDispatch)
        {
            try
            {
                FileResult fileResult;
                var output = context.Dispatchs
                            .Include(i => i.ClientMaster)
                            .Where(w => w.DispatchID == _oDispatch.DispatchID)
                            .FirstOrDefault();

                var company = context.Companys.FirstOrDefault();

                CommonLogic commonLogic = new CommonLogic();
                string body = commonLogic.CreateShippingBody(_hostingEnvironment.WebRootPath, output, company);
                fileResult = commonLogic.ConvertHtmlToPdf(body, output.DispatchNumber);

                return Json(fileResult);
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

        [Route("GetShippingAddress")]
        [HttpGet]
        public JsonResult GetShippingAddress(long clientId)
        {

            try
            {
                var output = context.ClientMasters
                                    .Include(i => i.CityMaster)
                                    .Include(i => i.DistrictMaster)
                                    .Include(i => i.StateMaster)
                                    .Where(w => w.ClientID == clientId)
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