using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using Microsoft.AspNetCore.Authorization;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Inventory Supervisor,Account Manager,Distributor")]
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        DB001Core context = new DB001Core();

        public IActionResult Index()
        {
            return View();
        }

        [Route("StockRegister")]
        [HttpGet]
        public JsonResult StockRegister()
        {
            DashboardStockRegister dashboard = new DashboardStockRegister();

            try
            {
                var orderCount = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => w.OrderStatus.Status == "Pending")
                    .Count();

                var pendingOrders = context.Orders
                    .Include(i => i.ClientMaster)
                    .Include(i => i.ProductMaster)
                    .Where(w => w.OrderStatus.Status == "Pending")
                    .ToList();

                var devices = context.DeviceMasters.ToList();
                var pendingDispatchCount = context.Dispatchs
                                        .Where(w => w.IsDispatched == false && w.IsDeleted  == false)
                                       .Count();
                var totalDispatchCount = context.Dispatchs
                        .Where(w => w.IsDispatched == true && w.IsDeleted == false)
                       .Count();

                var spares = context.SpareMasters.ToList();
                var inventoryCount = context.InventoryMasters.Count();
                var dispatchDetailCount = context.DispatchDetails.Count();

                var deviceCount = (from t in devices select t.Stock).Sum();
                var spareCount = (from t in spares select t.Stock).Sum();

                dashboard.PendingOrderCount = orderCount;
                dashboard.PendingDispatchCount = pendingDispatchCount;
                dashboard.AvailabaleInventory = inventoryCount - dispatchDetailCount;
                dashboard.AvailabaleDevices = deviceCount;
                dashboard.AvailabaleSpares = spareCount;

                dashboard.TotalDispatchCount = totalDispatchCount;

                dashboard.PendingOrders = pendingOrders;

                return Json(dashboard);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
                dashboard = null;
            }
        }

        [Route("AccontManager")]
        [HttpGet]
        public JsonResult AccontManager()
        {
            DashboardAccountManager dashboard = new DashboardAccountManager();

            try
            {
                var pendingOrderCount = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => w.OrderStatus.Status == "Device Ready")
                    .Count();

                var pendingDraftCount = context.Invoices
                    .Where(w =>  w.IsDraft == true && w.IsDeleted == false)
                    .Count();

                var pendingInvoiceCount = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Pending" && w.IsDraft == false && w.IsDeleted == false)
                    .Count();

                var unpaidInvoiceCount = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Unpaid" && w.IsDeleted == false)
                    .Count();

                var totalUnpaidAmount = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Unpaid" && w.IsDeleted == false)
                    .Sum(s => s.TotalAmount);

                var pendingOrders = context.Orders
                    .Include(i => i.ClientMaster)
                    .Include(i => i.ProductMaster)
                   .Include(i => i.OrderStatus)
                   .Where(w => w.OrderStatus.Status == "Device Ready")
                   .ToList();

                var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var saturday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
                
                var totalWeekendCollectionAmount = context.Invoices
                   .Where(w => w.PaymentStatus.Status == "Paid" && (w.PaymentDate >= sunday && w.PaymentDate <= saturday) && w.IsDeleted == false)
                   .Sum(s => s.TotalAmount);



                dashboard.PendingOrderCount = pendingOrderCount;
                dashboard.PendingDraftCount = pendingDraftCount;
                dashboard.PendingInvoiceCount = pendingInvoiceCount;
                dashboard.UnpaidInvoiceCount = unpaidInvoiceCount;
                dashboard.TotalUnpaidAmount = totalUnpaidAmount;
                dashboard.TotalWeekendCollectionAmount = totalWeekendCollectionAmount;

                dashboard.PendingOrders = pendingOrders;

                return Json(dashboard);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
                dashboard = null;
            }
        }

        [Route("Distributor")]
        [HttpGet]
        public JsonResult Distributor(long distributorId)
        {
            DashboardDistributor dashboard = new DashboardDistributor();

            try
            {
                var pendingOrderCount = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => (w.OrderStatus.Status == "Pending" || w.OrderStatus.Status == "Device Ready" || w.OrderStatus.Status == "Billing") && w.ClientMaster.DistributorID == distributorId && w.OrderStatus.Status != "Cancel")
                    .Count();

                var dispatchCount = context.Orders
                    .Where(w => (w.OrderStatus.Status == "Dispatched") && w.ClientMaster.DistributorID == distributorId && w.OrderStatus.Status != "Cancel")
                    .Count();

                var unpaidInvoiceCount = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Unpaid" && w.ClientMaster.DistributorID == distributorId && w.IsDeleted == false)
                    .Count();

                var cancelOrderCount = context.Orders
                  .Where(w => w.ClientMaster.DistributorID == distributorId && w.OrderStatus.Status == "Cancel")
                  .Count();





                var totalCommision = context.DistributorBills
                    .Where(w => w.PaymentStatus.Status == "Paid" && w.DistributorID == distributorId && w.IsDeleted == false)
                    .Sum(s => s.PaybleAmount);

                var pendingCommision = context.DistributorBills
                    .Where(w => w.PaymentStatus.Status == "Unpaid" && w.DistributorID == distributorId && w.IsDeleted == false)
                    .Sum(s => s.PaybleAmount);

                var totalClient = context.ClientMasters
                    .Where(w => w.DistributorID == distributorId && w.IsActive == true)
                    .Count();

                var totalCustomerSupport = 5;



                dashboard.PendingOrderCount = pendingOrderCount;
                dashboard.DispatchCount = dispatchCount;
                dashboard.UnpaidInvoiceCount = unpaidInvoiceCount;
                dashboard.CancelOrderCount = cancelOrderCount;

                dashboard.TotalCommision = totalCommision;
                dashboard.PendingCommision = pendingCommision;
                dashboard.TotalClient = totalClient;
                dashboard.TotalCustomerSupport = totalCustomerSupport;

                return Json(dashboard);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
                dashboard = null;
            }
        }

        [Route("MainDashboard")]
        [HttpGet]
        public JsonResult MainDashboard()
        {
            DashboardMain dashboard = new DashboardMain();

            try
            {
                var distributorCount = context.Distributors
                    .Where(w => (w.IsActive == true))
                    .Count();

                var clientCount = context.ClientMasters
                   .Where(w => (w.IsActive == true))
                    .Count();

                var productSoldCount = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => w.OrderStatus.Status == "Payment")
                    .Count();

                var paymentDues = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Unpaid"  && w.IsDeleted == false)
                    .Sum( s => s.TotalAmount);

                var callLogCount = 5;


                var pendingDeviceReadyCount = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => w.OrderStatus.Status == "Pending")
                    .Count();

                var pendingBillingCount = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => w.OrderStatus.Status == "Device Ready")
                    .Count();

                var pendingDispatchCount = context.Dispatchs
                    .Where(w => w.Order.OrderStatus.Status == "Billing" && w.IsDispatched == false)
                    .Count();

                var pendingPaymentCount = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Unpaid" && w.IsDeleted == false)
                    .Count();

                var totalCollection = context.Invoices
                    .Where(w => w.PaymentStatus.Status == "Paid" && w.IsDeleted == false)
                    .Sum(s => s.TotalAmount);

                var totalProfit = 100;

                var orderQueue = context.Orders
                    .Include(i => i.OrderStatus)
                    .Where(w => w.OrderStatus.Status != "Cancel" && w.OrderStatus.Status != "Payment")
                    .Count();

                var userCount = context.UserMasters
                    .Where(w => w.IsActive == true)
                    .Count();


                dashboard.DistributorCount = distributorCount;
                dashboard.ClientCount = clientCount;
                dashboard.ProductSoldCount = productSoldCount;
                dashboard.PaymentDues = paymentDues;
                dashboard.CallLogCount = callLogCount;

                dashboard.PendingDeviceReadyCount = pendingDeviceReadyCount;
                dashboard.PendingBillingCount = pendingBillingCount;
                dashboard.PendingDispatchCount = pendingDispatchCount;
                dashboard.PendingPaymentCount = pendingPaymentCount;

                dashboard.TotalCollection = totalCollection;
                dashboard.TotalProfit = totalProfit;
                dashboard.OrderQueue = orderQueue;
                dashboard.UserCount = userCount;


                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                var from = month.AddMonths(-12);

                ///Order Chart
                List<MonthlyChart> OrdersChart = new List<MonthlyChart>();

                var fromDate = month;
                for (int i = 11; i >= 0; i--)
                {
                    
                    var date = fromDate.AddMonths(-i);

                    var count = context.Orders
                                .Where(w => w.CreatedOn.Value.Month == date.Month && w.CreatedOn.Value.Year == date.Year && w.OrderStatus.Status != "Cancel")
                                .Count();

                    OrdersChart.Add(new MonthlyChart{
                        Name = "OrdersChart",
                        iYear = date.Year,
                        sMonth = date.ToString("MMM"),
                        Count = count
                    });

                }

                dashboard.OrdersChart = OrdersChart;

                ///End Order Chart

                ///Dispatch Chart
                List<MonthlyChart> DispatchChart = new List<MonthlyChart>();

                
                for (int i = 11; i >= 0; i--)
                {

                    var date = fromDate.AddMonths(-i);

                    var count = context.Dispatchs
                                .Where(w => w.CreatedOn.Value.Month == date.Month && w.CreatedOn.Value.Year == date.Year && w.IsDeleted == false && w.IsDispatched == true)
                                .Count();

                    DispatchChart.Add(new MonthlyChart
                    {
                        Name = "DispatchChart",
                        iYear = date.Year,
                        sMonth = date.ToString("MMM"),
                        Count = count
                    });

                }

                dashboard.DispatchChart = DispatchChart;

                ///End Dispatch Chart

                ///PaymentDues Chart
                List<MonthlyChart> PaymentDuesChart = new List<MonthlyChart>();


                for (int i = 11; i >= 0; i--)
                {

                    var date = fromDate.AddMonths(-i);

                    var count = context.Invoices
                                .Where(w => w.CreatedOn.Month == date.Month && w.CreatedOn.Year == date.Year && w.IsDeleted == false && w.PaymentStatus.Status == "UnPaid")
                                .Count();

                    PaymentDuesChart.Add(new MonthlyChart
                    {
                        Name = "PaymentDuesChart",
                        iYear = date.Year,
                        sMonth = date.ToString("MMM"),
                        Count = count
                    });

                }

                dashboard.PaymentDuesChart = PaymentDuesChart;

                ///End PaymentDues Chart

                ///CallLogsChart
                //List<MonthlyChart> PaymentDuesChart = new List<MonthlyChart>();


                //for (int i = 11; i >= 0; i--)
                //{

                //    var date = fromDate.AddMonths(-i);

                //    var count = context.Invoices
                //                .Where(w => w.CreatedOn.Month == date.Month && w.CreatedOn.Year == date.Year && w.IsDeleted == false && w.PaymentStatus.Status == "UnPaid")
                //                .Count();

                //    PaymentDuesChart.Add(new MonthlyChart
                //    {
                //        Name = "PaymentDuesChart",
                //        iYear = date.Year,
                //        sMonth = date.ToString("MMM"),
                //        Count = count
                //    });

                //}

                //dashboard.PaymentDuesChart = PaymentDuesChart;

                ///End PaymentDues Chart



                return Json(dashboard);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                context = null;
                dashboard = null;
            }
        }
    }
}