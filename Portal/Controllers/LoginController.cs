using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using Portal.Common;
using System;
using Microsoft.AspNetCore.Authorization;
using Portal.Configuration;

namespace Portal.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        DB001Core context = new DB001Core();

        public IActionResult Index()
        {
            return View();
        }

        [Route("ValidateUser")]
        [HttpPost]
        public JsonResult ValidateUser([FromBody] UserMaster _oUserMaster)
        {

            try
            {
                UserManagement userManagement = new UserManagement();
                var user = context.UserMasters
                     .Where(u => u.UserName == _oUserMaster.UserName && u.Password == _oUserMaster.Password)
                     .Include(p => p.ProfileMaster)
                     .FirstOrDefault();


                userManagement.UserMaster = user;
                if (user != null)
                {
                    userManagement.Status = "Success";
                    var menu = context.MenuProfileLinks
                        .Where(p => p.ProfileID == user.ProfileID)
                         .Include(m => m.MenuMaster)
                         .ToList();

                    List<MenuMaster> listOfMenuMaster = new List<MenuMaster>();
                    foreach(var item in menu)
                    {
                        if(item.MenuMaster.IsActive)
                            listOfMenuMaster.Add(item.MenuMaster);
                    }

                    userManagement.ListOfMenuMaster = listOfMenuMaster;
                }
                else
                {
                    userManagement.Status = "Invalid";
                }

                return Json(userManagement);
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