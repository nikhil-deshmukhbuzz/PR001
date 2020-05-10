using Portal.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using SelectPdf;
using Microsoft.AspNetCore.Mvc;
using Portal.Models;

namespace Portal.Common
{
    public class CommonLogic
    {
        DB001Core context = new DB001Core();

        

        public CommonLogic()
        { 
        }

        public string GenerateClientCode(string input)
        {
            string Code = "";
            var ClientCodeList = input.Split(' ').Select(i => i[0]).ToList();

            for (int i = 0; i < ClientCodeList.Count; i++)
            {
                Code += ClientCodeList[i];

            }

            bool ClientExists = ClientCodeExistance(Code.ToUpper());
            int j = 65;
            while (ClientExists)
            {
                char c = (char)j;
                Code = Code + c.ToString();
                ClientExists = ClientCodeExistance(Code.ToUpper());
            }

            return Code.ToUpper();
        }


        private bool ClientCodeExistance(string ClientCode)
        {
            var client = context.ClientMasters
               .Where(s => s.ClientCode == ClientCode)
               .FirstOrDefault();

            if (client != null)
                return true;
            else
                return false;
        }

        public string GenerateOrderNumber()
        {
            string Code = "";
            var NoOfInvoice = context.Orders.Count() + 1;



            Code = "ORD-" + String.Format("{0:000000}", NoOfInvoice); ;

            return Code.ToUpper();
        }

        public string GenerateDistributorCode()
        {
            string Code = "";


            var NoOfDispatch = context.Distributors.Count() + 1;

            Code = "DIST-" + String.Format("{0:000000}", NoOfDispatch);

            return Code.ToUpper();
        }

        public string GenerateDispatchCode()
        {
            string Code = "";
            

            var NoOfDispatch = context.Dispatchs.Count() + 1;

            Code = "D-" + String.Format("{0:000000}", NoOfDispatch);

            return Code.ToUpper();
        }

        public string GenerateInvoiceNumber()
        {
            string Code = "";
            var NoOfInvoice = context.Invoices.Count() + 1;

            Code = "INV-" + String.Format("{0:000000}", NoOfInvoice);

            return Code.ToUpper();
        }

        public string GenerateDistributorBillNumber()
        {
            string Code = "";
            var NoOfInvoice = context.DistributorBills.Count() + 1;

            Code = "DB-" + String.Format("{0:000000}", NoOfInvoice);

            return Code.ToUpper();
        }
        
        public int ChangeOrderStatus(long orderId, string newStatus)
        {
            var output = 0;
            long currentStatusId = 0;
            long newStatusId = 0;

            var oCurrentStatus = context.Orders
               .Where(w => w.OrderID == orderId)
               .FirstOrDefault();

            var oNewStatus = context.OrderStatuss
               .Where(w => w.Status == newStatus)
               .FirstOrDefault();

            if (oCurrentStatus != null && oNewStatus != null)
            {
                currentStatusId = oCurrentStatus.OrderStatusID;
                newStatusId = oNewStatus.OrderStatusID;

                if (currentStatusId < newStatusId)
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.Orders
                          .Where(w => w.OrderID == orderId)
                          .FirstOrDefault();

                        input.OrderStatusID = newStatusId;
                        output = context.SaveChanges();
                    }
                }
            }
            return output;
        }

        public string CreateInvoiceBody(string path,Invoice invoice,Company company)
        {
            string body = string.Empty;
            var file_path = path + "\\templates\\InvoiceTemplate.html";
            
            using (StreamReader reader = new StreamReader(file_path))
            {
                body = reader.ReadToEnd();
            }

            #region Company
            body = body.Replace("{company_name}", company.Name);
            body = body.Replace("{company_logo}", company.Logo);
            body = body.Replace("{company_address}", company.RegisteredAddress);
            body = body.Replace("{company_warehouse}", company.WarehouseAddress);
            body = body.Replace("{company_phone_fax}", company.ContactNumber);
            body = body.Replace("{company_email_web}", company.Website+" ● "+ company.Email);
            body = body.Replace("{payment_info1}", "Payments,");
            body = body.Replace("{payment_info2}", "ACCOUNT NUMBER — 123006705");
            body = body.Replace("{payment_info3}", " ● IBAN — US100000060345");
            body = body.Replace("{payment_info4}", " ● SWIFT — BOA447");
            body = body.Replace("{payment_info5}", "");
            body = body.Replace("{issue_date_label}", "Issue Date,");
            body = body.Replace("{issue_date}",  DateTime.Now.ToString("dd-MMM-yyyy"));
            body = body.Replace("{due_date_label}", "Due Date,");
            body = body.Replace("{due_date}", DateTime.Today.AddDays(7).ToString("dd-MMM-yyyy"));
            body = body.Replace("{currency_label}", "* All prices are in");
            body = body.Replace("{currency}", "INR");
            body = body.Replace("{order_number_label}", "Order No.");
            body = body.Replace("{order_number}", invoice.Order.OrderNumber);
            #endregion

            #region BillTo
            body = body.Replace("{bill_to_label}", "Bill to,");
            body = body.Replace("{client_name}", invoice.ClientMaster.ClientName);
            body = body.Replace("{client_person}", invoice.ClientMaster.ContactPerson);
            body = body.Replace("{client_address}", invoice.ClientMaster.Address);

            string city_district_state_pincode = invoice.ClientMaster.CityMaster.CityName + "," + invoice.ClientMaster.DistrictMaster.DistrictName + "," + invoice.ClientMaster.StateMaster.StateName+"-"+ invoice.ClientMaster.CityMaster.Picode;
            body = body.Replace("{city_district_state_pincode}", city_district_state_pincode);
            body = body.Replace("{client_phone_fax}", "+91-"+ invoice.ClientMaster.MobileNo);
            body = body.Replace("{client_email}", invoice.ClientMaster.Email);

            #endregion

            body = body.Replace("{invoice_title}", "Invoice");
            body = body.Replace("{invoice_number}", invoice.InvoiceNumber);

            #region BillDetail
            body = body.Replace("{item_row_number_label}", "");
            body = body.Replace("{item_refno_label}", "Ref. No");
            body = body.Replace("{item_description_label}", "Item");
            body = body.Replace("{item_line_total_label}", "Total");

            string strItem = "";

            int i = 1;
            decimal subtotal = 0;
            foreach (var item in invoice.InvoiceDetails)
            {
                if (item.InventoryMaster != null)
                {
                    if (item.InventoryMaster.DeviceMaster != null)
                    {
                        strItem += "<tr>";

                        strItem += "<td>" + i + "</td>";
                        strItem += "<td> #" + item.InventoryMaster.ReferenceNumber + "</td>";
                        strItem += "<td>" + item.InventoryMaster.DeviceMaster.DeviceName + "</td>";
                        strItem += "<td>" + item.Amount + "</td>";

                        strItem += "</tr>";

                        subtotal = subtotal + item.Amount;
                    }

                    if (item.InventoryMaster.SpareMaster != null)
                    {
                        strItem += "<tr>";

                        strItem += "<td>" + i + "</td>";
                        strItem += "<td> #" + item.InventoryMaster.ReferenceNumber + "</td>";
                        strItem += "<td>" + item.InventoryMaster.SpareMaster.SpareName + "</td>";
                        strItem += "<td>" + item.Amount + "</td>";

                        strItem += "</tr>";

                        subtotal = subtotal + item.Amount;
                    }
                    i++;
                }
            }
            #endregion

            #region Total
            string strHeaders = "";

            foreach (var item in invoice.InvoiceDetails)
            {
                if (item.InvoiceHeader != null)
                {
                    strHeaders += "<tr>";

                    strHeaders += "<th>" + item.InvoiceHeader.Header + "</th>";
                    strHeaders += "<td>" + item.InvoiceHeader.Amount + "</td>";

                    strHeaders += "</tr>";
                }
            }
            

            body = body.Replace("{item_details}", strItem);
            body = body.Replace("{tax_details}", strHeaders);

            body = body.Replace("{amount_subtotal_label}", "Subtotal,");
            body = body.Replace("{amount_subtotal}", subtotal.ToString());
            body = body.Replace("{tax_name}", "Tax,");
            body = body.Replace("{tax_value}", "");
            body = body.Replace("{amount_total_label}", "Total,");
            body = body.Replace("{amount_total}", invoice.TotalAmount.ToString());
            body = body.Replace("{terms_label}", "Terms & Notes");
            body = body.Replace("{terms}", invoice.ClientMaster.ContactPerson + ", thank you very much. We really appreciate your business.<br />Please send payments before the due date.");

            #endregion

            return body;
        }


        public string CreateShippingBody(string path,Dispatch dispatch ,Company company)
        {
            string body = string.Empty;
            var file_path = path + "\\templates\\shippingTemplate.html";

            using (StreamReader reader = new StreamReader(file_path))
            {
                body = reader.ReadToEnd();
            }

            #region From
            body = body.Replace("{company_name}", company.Name);
            body = body.Replace("{company_logo}", company.Logo);
            body = body.Replace("{company_address}", company.RegisteredAddress);
            body = body.Replace("{company_warehouse}", company.WarehouseAddress);
            body = body.Replace("{company_phone_fax}", company.ContactNumber);
            #endregion

            #region To
            body = body.Replace("{client_name}", dispatch.ClientMaster.ClientName);
            body = body.Replace("{client_person}", dispatch.ClientMaster.ContactPerson);
            body = body.Replace("{client_address}", dispatch.ShippingAddress);
            body = body.Replace("{client_mobileNo}", dispatch.ClientMaster.MobileNo);
            #endregion



            return body;
        }

        public FileResult ConvertHtmlToPdf(string body,string fileName)
        {
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(body);
            byte[] pdf = doc.Save();
            doc.Close();
            
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = fileName;
            return fileResult;
        }
    }

}
