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
    [Authorize(Roles = "Superadmin,Inventory Supervisor")]
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        DB001Core context = new DB001Core();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public int Add([FromBody] InventoryMaster _oInventoryMaster)
        {

            try
            {
                int output;
                bool IsStockAvailable = true;

                if(_oInventoryMaster.DeviceID != null)
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.DeviceMasters
                                       .Where(s => s.DeviceID == _oInventoryMaster.DeviceID)
                                       .FirstOrDefault();

                        if (input.Stock <= 0)
                        {
                            IsStockAvailable = false;

                        }
                        else
                        {
                            input.Stock = input.Stock - 1;
                            output = context.SaveChanges();
                        }
                    }
                }

                if (_oInventoryMaster.SpareID != null)
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.SpareMasters
                                       .Where(s => s.SpareID == _oInventoryMaster.SpareID)
                                       .FirstOrDefault();

                        if (input.Stock <= 0)
                        {
                            IsStockAvailable = false;

                        }
                        else
                        {
                            input.Stock = input.Stock - 1;
                            output = context.SaveChanges();
                        }
                    }
                }


                if (IsStockAvailable)
                {
                    using (var context = new DB001Core())
                    {
                        context.InventoryMasters.Add(_oInventoryMaster);
                        output = context.SaveChanges();
                    }
                }
                else
                {
                    output = -1;
                }
                return output;
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
        public bool Update([FromBody] InventoryMaster _oInventoryMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.InventoryMasters
                                   .Where(s => s.InventoryID == _oInventoryMaster.InventoryID)
                                   .FirstOrDefault();

                    input.ReferenceNumber = _oInventoryMaster.ReferenceNumber;
                    input.InventoryType = _oInventoryMaster.InventoryType;
                    input.WarrantyDate = _oInventoryMaster.WarrantyDate;
                    input.DeviceID = _oInventoryMaster.DeviceID;
                    input.SpareID = _oInventoryMaster.SpareID;
                    
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
        [HttpGet]
        public JsonResult GetList()
        {

            try
            {
                var output = context.InventoryMasters
                                           .Include(d =>d.DeviceMaster)
                                           .Include(d => d.SpareMaster)
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
        public JsonResult Get(long inventoryId)
        {

            try
            {
                var output = context.InventoryMasters
                                     .Where(s => s.InventoryID == inventoryId)
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

        [Route("GetDeviceSpareList")]
        [HttpGet]
        public JsonResult GetDeviceSpareList()
        {

            try
            {
                var devices = context.DeviceMasters.ToList();
                var spares = context.SpareMasters.ToList();

                List<DeviceMaster> listOfDeviceMaster = new List<DeviceMaster>();
                
                foreach (var item in devices)
                {
                    listOfDeviceMaster.Add(new DeviceMaster {
                        DeviceID = item.DeviceID,
                        DeviceName = item.DeviceName,
                        HardwareType = "D"
                    });
                }

                foreach (var item in spares)
                {
                    listOfDeviceMaster.Add(new DeviceMaster
                    {
                        DeviceID = item.SpareID,
                        DeviceName = item.SpareName,
                        HardwareType = "S"
                    });
                }

                var output = listOfDeviceMaster;
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

        [Route("GetHardwareList")]
        [HttpGet]
        public JsonResult GetHardwareList()
        {

            try
            {
                var output = context.HardwareMasters.ToList();
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

        [Route("Existance")]
        [HttpGet]
        public JsonResult Existance(long inventoryId)
        {

            try
            {
                bool output = false;
                var inventory = context.DispatchDetails
                     .Where(s => s.InventoryID == inventoryId)
                     .FirstOrDefault();
                if (inventory != null)
                    output = true;
                else
                    output = false;

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
        public bool Delete([FromBody] InventoryMaster _oInventoryMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    context.Remove<InventoryMaster>(_oInventoryMaster);

                    output = context.SaveChanges();
                }

                if (_oInventoryMaster.DeviceID != null)
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.DeviceMasters
                                       .Where(s => s.DeviceID == _oInventoryMaster.DeviceID)
                                       .FirstOrDefault();

                        input.Stock = input.Stock + 1;

                        output = context.SaveChanges();
                    }
                }

                if (_oInventoryMaster.SpareID != null)
                {
                    using (var context = new DB001Core())
                    {
                        var input = context.SpareMasters
                                       .Where(s => s.SpareID == _oInventoryMaster.SpareID)
                                       .FirstOrDefault();

                        input.Stock = input.Stock + 1;

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
    }
}