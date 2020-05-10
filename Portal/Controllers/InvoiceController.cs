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

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Account Manager")]
    [Route("[controller]")]
    public class InvoiceController : Controller
    {
        DB001Core context = new DB001Core();

        private readonly IHostingEnvironment _hostingEnvironment;

        public InvoiceController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] Invoice _oInvoice)
        {
            try
            {
                int output;

                CommonLogic clsCommonLogic = new CommonLogic();
                _oInvoice.InvoiceNumber = clsCommonLogic.GenerateInvoiceNumber();

                _oInvoice.CreatedOn = DateTime.Now;
                _oInvoice.ModifiedOn = DateTime.Now;
                _oInvoice.IsDeleted = false;

                using (var context = new DB001Core())
                {
                    context.Invoices.Add(_oInvoice);
                    output = context.SaveChanges();
                }

                if (!_oInvoice.IsDraft)
                {
                    using (var context = new DB001Core())
                    {

                        var input = context.Orders
                            .Include(i => i.OrderStatus)
                            .Where(o => o.OrderID == _oInvoice.OrderID && (o.OrderStatus.Status == "Pending" || o.OrderStatus.Status == "Device Ready" || o.OrderStatus.Status == "Delivered"))
                            .FirstOrDefault();

                        if (input != null)
                        {
                            if (input.OrderStatus.Status == "Pending" || input.OrderStatus.Status == "Device Ready")
                                input.OrderStatusID = 3;
                            else if (input.OrderStatus.Status == "Delivered" || _oInvoice.PaymentStatusID == 1) //Paid
                                input.OrderStatusID = 5;

                            var status = context.SaveChanges();
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

        [Route("Update")]
        [HttpPost]
        public bool Update([FromBody] Invoice _oInvoice)
        {
            try
            {
                int output = 0;
                if (_oInvoice.IsDraft)
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.Invoices
                                       .Include(i => i.InvoiceDetails)
                                       .Where(w => w.InvoiceID == _oInvoice.InvoiceID)
                                       .FirstOrDefault();

                        input.ClientID = _oInvoice.ClientID;
                        input.ProductID = _oInvoice.ProductID;
                        input.OrderID = _oInvoice.OrderID;
                        input.PaymentModeID = _oInvoice.PaymentModeID;
                        input.PaymentStatusID = _oInvoice.PaymentStatusID;
                        input.TotalAmount = _oInvoice.TotalAmount;
                        input.PaymentDate = _oInvoice.PaymentDate;
                        input.InvoiceDetails = _oInvoice.InvoiceDetails;
                        input.IsDraft = _oInvoice.IsDraft;
                        
                        output = context.SaveChanges();
                    }
                }
                else
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.Invoices
                                       .Include(i => i.InvoiceDetails)
                                       .Where(w => w.InvoiceID == _oInvoice.InvoiceID)
                                       .FirstOrDefault();

                        if (_oInvoice.PaymentStatusID == 1 && input.PaymentDate == null)
                        {
                            input.PaymentDate = DateTime.Now;
                            AddDistributorBill(_oInvoice);
                        }

                        input.PaymentModeID = _oInvoice.PaymentModeID;
                        input.PaymentStatusID = _oInvoice.PaymentStatusID;
                        input.IsDraft = _oInvoice.IsDraft;
                        input.InvoiceDetails = _oInvoice.InvoiceDetails;

                        output = context.SaveChanges();

                        if (_oInvoice.PaymentStatusID == 1 && input.PaymentDate == null)
                        {
                            var invoice = context.Invoices
                                .Where(w => w.OrderID == _oInvoice.OrderID)
                                .FirstOrDefault();

                            AddDistributorBill(invoice);
                        }
                    }

                    if(_oInvoice.PaymentStatusID != 1)
                    {
                        CommonLogic commonLogic = new CommonLogic();
                        commonLogic.ChangeOrderStatus(_oInvoice.OrderID, "Billing");
                    }

                    else if (_oInvoice.PaymentStatusID == 1)
                    {
                       
                        CommonLogic commonLogic = new CommonLogic();
                        commonLogic.ChangeOrderStatus(_oInvoice.OrderID, "Payment");
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
                var output = context.Invoices
                    .Include(i => i.ClientMaster)
                    .Include(i => i.Order)
                    .Include(i => i.PaymentStatus)
                    .OrderByDescending(o =>  o.PaymentStatus.Status)
                    .ThenByDescending(o => o.CreatedOn)
                    .Where(w => w.IsDeleted == false)
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
        public JsonResult Get(long invoiceId)
        {

            try
            {
                var output = context.Invoices
                                     .Where(s => s.InvoiceID == invoiceId)
                                     .FirstOrDefault();


                output.InvoiceDetails = context.InvoiceDetails
                   .Include(i => i.InventoryMaster.DeviceMaster)
                   .Include(i => i.InventoryMaster.SpareMaster)
                   .Include(i => i.InvoiceHeader)
                   .Where(w => w.InvoiceID == invoiceId)
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

        [Route("GetOrderList")]
        [HttpGet]
        public JsonResult GetOrderList(long clientId, long productId,bool isEditable)
        {
            try
            {
                if (!isEditable)
                {
                   var output = context.Orders
                        .Where(w => w.ClientID == clientId && w.ProductID == productId && w.OrderStatus.Status == "Device Ready")
                        .Include(i => i.OrderStatus)
                        .ToList();

                    var invoice = context.Invoices
                        .Include(i => i.Order)
                        .Where(w => w.IsDeleted == false)
                        .ToList();

                    List<Order> Order = new List<Order>();

                    foreach (var item in output)
                    {
                        var order = invoice.SingleOrDefault(w => w.OrderID == item.OrderID);

                        if(order == null)
                        {
                            Order.Add(item);
                        }
                    }
                        return Json(Order);
                }
                else
                {
                   var  output = context.Orders
                        .Where(w => w.ClientID == clientId && w.ProductID == productId)
                        .Include(i => i.OrderStatus)
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

        [Route("GetPaymentMode")]
        [HttpGet]
        public JsonResult GetPaymentMode()
        {
            try
            {

                var output = context.PaymentModes
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

        [Route("GetInvoiceHeaders")]
        [HttpGet]
        public JsonResult GetInvoiceHeaders()
        {
            try
            {

                var output = context.InvoiceHeaders
                    .Where(w => w.IsActive == true)
                    .OrderBy(o => o.SequenceNo)
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

        [Route("GetPaymentStatus")]
        [HttpGet]
        public JsonResult GetPaymentStatus()
        {
            try
            {

                var output = context.PaymentStatuss
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
        public JsonResult GetInventoryList(long orderId)
        {
            try
            {

                var output = context.DispatchDetails
                    .Include(i => i.InventoryMaster.DeviceMaster)
                    .Include(i => i.InventoryMaster.SpareMaster)
                    .Where(w => w.Dispatch.Order.OrderID == orderId && w.Dispatch.IsDeleted == false)
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

        [Route("MakeInvoice")]
        [HttpPost]
        public JsonResult MakeInvoice([FromBody] Invoice _oInvoice)
        {
            try
            {
                FileResult fileResult;
                var output = context.Invoices
                            .Include(i => i.Order)
                            .Include(i => i.ClientMaster)
                            .Include(i => i.ClientMaster.StateMaster)
                            .Include(i => i.ClientMaster.DistrictMaster)
                            .Include(i => i.ClientMaster.CityMaster)
                            .Include(i => i.ProductMaster)
                            .Where(w => w.InvoiceID == _oInvoice.InvoiceID && w.IsDraft == false)
                            .FirstOrDefault();

                if (output != null)
                {
                    output.InvoiceDetails = context.InvoiceDetails
                        .Include(i => i.InventoryMaster)
                       .Include(i => i.InventoryMaster.DeviceMaster)
                       .Include(i => i.InventoryMaster.SpareMaster)
                       .Include(i => i.InvoiceHeader)
                       .Where(w => w.InvoiceID == _oInvoice.InvoiceID)
                       .ToList();

                    if (output.InvoiceDetails != null)
                    {
                        var company = context.Companys.FirstOrDefault();

                        CommonLogic commonLogic = new CommonLogic();
                        string body = commonLogic.CreateInvoiceBody(_hostingEnvironment.WebRootPath, output, company);
                        fileResult = commonLogic.ConvertHtmlToPdf(body, output.InvoiceNumber);
                    }
                    else
                    {
                        fileResult = null;
                    }
                }
                else
                {
                    fileResult = null;
                }

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


        [Route("AddDistributorBill")]
        [HttpGet]
        public void AddDistributorBill(Invoice _oInvoice)
        {
            var order = context.Orders
                .Include(i => i.ClientMaster.Distributor)
                .Where(w => w.OrderID == _oInvoice.OrderID)
                .FirstOrDefault();

            DistributorBill distributorBill = new DistributorBill();

            if (order != null && order.ClientMaster.DistributorID != null)
            {
                CommonLogic clsCommonLogic = new CommonLogic();
                distributorBill.BillNumber = clsCommonLogic.GenerateDistributorBillNumber();
                distributorBill.DistributorID = order.ClientMaster.Distributor.DistributorID;
                distributorBill.ClientID = order.ClientID;
                distributorBill.ProductID = order.ProductID;
                distributorBill.OrderID = order.OrderID;
                distributorBill.InvoiceID = _oInvoice.InvoiceID;

                decimal productAmount = context.InvoiceHeaders
                    .Where(w => w.ProductID == order.ProductID && w.IsSoftware == true)
                    .FirstOrDefault().Amount;

                decimal commision = context.ProductMasters
                    .Where(w => w.ProductID == _oInvoice.ProductID)
                    .FirstOrDefault().Commision;

                decimal totalAmount = productAmount * (commision / 100);

                distributorBill.Commision = commision;
                distributorBill.ProductAmount = _oInvoice.TotalAmount;
                distributorBill.PaybleAmount = totalAmount;
                distributorBill.PaymentStatusID = 2;

                distributorBill.CreatedOn = DateTime.Now;
                distributorBill.ModifiedOn = DateTime.Now;
                distributorBill.IsDeleted = false;

                using (var context = new DB001Core())
                {
                    context.DistributorBills.Add(distributorBill);
                    int output = context.SaveChanges();
                }
            }
        }
    }

}