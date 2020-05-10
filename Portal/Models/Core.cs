using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Core
    {
    }

    public class UserManagement
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string Status { get; set; }
        public UserMaster UserMaster { get; set; }
        public List<MenuMaster> ListOfMenuMaster { get; set; }
    }

    public class DashboardStockRegister
    {
        public int PendingOrderCount { get; set; }
        public int PendingDispatchCount { get; set; }
        public int AvailabaleDevices { get; set; }
        public int AvailabaleSpares { get; set; }
        public int AvailabaleInventory { get; set; }

        public int TotalFund { get; set; }
        public int TotalDispatchCount { get; set; }

        public List<Order> PendingOrders { get; set; }
    }

    public class DashboardAccountManager
    {
        public int PendingOrderCount { get; set; }
        public int PendingDraftCount { get; set; }
        public int PendingInvoiceCount { get; set; }
        public int UnpaidInvoiceCount { get; set; }
       

        public decimal TotalUnpaidAmount { get; set; }
        public decimal TotalWeekendCollectionAmount { get; set; }

        public List<Order> PendingOrders { get; set; }
    }

    public class DashboardDistributor
    {
        public int PendingOrderCount { get; set; }
        public int DispatchCount { get; set; }
        public int UnpaidInvoiceCount { get; set; }
        public int CancelOrderCount { get; set; }


        public decimal TotalCommision { get; set; }
        public decimal PendingCommision { get; set; }
        public int TotalClient { get; set; }
        public int TotalCustomerSupport { get; set; }
    }

    public class DashboardMain
    {
        public int DistributorCount { get; set; }
        public int ClientCount { get; set; }
        public int ProductSoldCount { get; set; }
        public decimal PaymentDues { get; set; }
        public int CallLogCount { get; set; }


        public int PendingDeviceReadyCount { get; set; }
        public int PendingBillingCount { get; set; }
        public int PendingDispatchCount { get; set; }
        public int PendingPaymentCount { get; set; }

        public decimal TotalCollection { get; set; }
        public decimal TotalProfit { get; set; }
        public int OrderQueue { get; set; }
        public int UserCount { get; set; }

        public List<MonthlyChart> OrdersChart { get; set; }
        public List<MonthlyChart> DispatchChart { get; set; }
        public List<MonthlyChart> PaymentDuesChart { get; set; }
        public List<MonthlyChart> CallLogsChart { get; set; }
    }

    public class MonthlyChart
    {
        public string Name { get; set; }
        public string sYear { get; set; }
        public string sMonth { get; set; }
        public int iYear { get; set; }
        public int iMonth { get; set; }
        public int Count { get; set; }
    }
}
