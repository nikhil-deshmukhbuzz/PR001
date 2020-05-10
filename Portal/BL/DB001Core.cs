using Microsoft.EntityFrameworkCore;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.BL
{
   public class DB001Core: DbContext
    {
        public DB001Core()
        {
        }

        public DB001Core(DbContextOptions<DB001Core> options)
            : base(options)
        {
        }

        public DbSet<DeviceMaster> DeviceMasters { get; set; }
        public DbSet<SpareMaster> SpareMasters { get; set; }
        public DbSet<InventoryMaster> InventoryMasters { get; set; }
        public DbSet<HardwareMaster> HardwareMasters { get; set; }
        public DbSet<CityMaster> CityMasters { get; set; }
        public DbSet<DistrictMaster> DistrictMasters { get; set; }
        public DbSet<StateMaster> StateMasters { get; set; }
        public DbSet<ClientMaster> ClientMasters { get; set; }
        public DbSet<ProductMaster> ProductMasters { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<OrderStatus> OrderStatuss { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Dispatch> Dispatchs { get; set; }
        public DbSet<DispatchDetail> DispatchDetails { get; set; }
        public DbSet<MenuMaster> MenuMasters { get; set; }
        public DbSet<ProfileMaster> ProfileMasters { get; set; }
        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<MenuProfileLink> MenuProfileLinks { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<PaymentStatus> PaymentStatuss { get; set; }
        public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorBill> DistributorBills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=NIKHIL\SQLEXPRESS;Database=DB001Core;Trusted_Connection=True;");
        }

    }

}
