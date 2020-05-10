using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using Portal.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    [Route("[controller]")]
    public class UserManagementController : Controller
    {
        DB001Core context = new DB001Core();
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        //public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        //{
        //    this.userManager = userManager;
        //    this.roleManager = roleManager;
        //}

        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public bool Add([FromBody] UserMaster _oUserMaster)
        {

            try
            {
                int output = 0;
                using (var context = new DB001Core())
                {
                    context.UserMasters.Add(_oUserMaster);
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
        public async Task<bool> Update([FromBody] UserMaster _oUserMaster)
        {
            try
            {
                int output = 0;

                UserViewModel userViewModel = new UserViewModel
                {
                    Name = _oUserMaster.Name,
                    UserName = _oUserMaster.UserName,
                    Password = _oUserMaster.Password
                };

                ApplicationUser applicationUser = applicationDbContext.Users
                    .Where(w => w.UserName == _oUserMaster.UserName)
                    .FirstOrDefault();
                if (applicationUser != null)
                {
                    UserController userController = new UserController(userManager, roleManager);
                    var appOutput = await userController.UpdateUser(applicationUser.Id, userViewModel);

                    if (Convert.ToBoolean(appOutput.Value))
                    {
                        using (var context = new DB001Core())
                        {
                            var input = context.UserMasters
                                           .Where(s => s.UserID == _oUserMaster.UserID)
                                           .FirstOrDefault();

                            if (_oUserMaster.Password != "")
                                input.Password = _oUserMaster.Password;

                            input.Name = _oUserMaster.Name;
                            input.MobileNo = _oUserMaster.MobileNo;
                            input.IsActive = _oUserMaster.IsActive;

                            output = context.SaveChanges();
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

        [Route("UpdatePassword")]
        [HttpPost]
        public async Task<bool> UpdatePassword([FromBody] UserMaster _oUserMaster)
        {
            try
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    Password = _oUserMaster.Password
                };

                ApplicationUser applicationUser = applicationDbContext.Users
                    .Where(w => w.UserName == _oUserMaster.UserName)
                    .FirstOrDefault();
                if (applicationUser != null)
                {
                    UserController userController = new UserController(userManager, roleManager);
                    var output = await userController.UpdatePassword(applicationUser.Id, userViewModel);

                    if (Convert.ToBoolean(output.Value))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
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

        [Route("UpdateStatus")]
        [HttpPost]
        public bool UpdateStatus([FromBody] UserMaster _oUserMaster)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.UserMasters
                                   .Where(s => s.UserID == _oUserMaster.UserID)
                                   .FirstOrDefault();

                    input.IsActive = _oUserMaster.IsActive;

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
                var output = context.UserMasters
                    .Include(i => i.ProfileMaster)
                    .Where(
                           w => w.ProfileMaster.ProfileName != "Superadmin" &&
                           w.ProfileMaster.ProfileName != "Administrator" &&
                           w.ProfileMaster.ProfileName != "Inventory Supervisor" &&
                           w.ProfileMaster.ProfileName != "Account Manager"
                           )
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
        public JsonResult Get(long userId)
        {

            try
            {
                var output = context.UserMasters
                            .Include(i => i.ProfileMaster)
                            .Where(w => w.UserID == userId)
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

        [Route("GetProfileMasterList")]
        [HttpGet]
        public JsonResult GetProfileMasterList()
        {

            try
            {
                var output = context.ProfileMasters
                    .Where(
                           w => w.ProfileName != "Superadmin" &&
                           w.ProfileName != "Administrator" &&
                           w.ProfileName != "Inventory Supervisor" &&
                           w.ProfileName != "Account Manager"
                           )
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

        [Route("GetDistributorList")]
        [HttpGet]
        public JsonResult GetDistributorList(bool isEditable)
        {

            try
            {
                if (!isEditable)
                {
                    var distributor = context.Distributors
                         .Where(w => w.IsActive == true)
                         .ToList();

                    var user = context.UserMasters
                        .Include(i => i.Distributor)
                        .ToList();

                    List<Distributor> Distributor = new List<Distributor>();

                    foreach (var item in distributor)
                    {
                        var order = user.SingleOrDefault(w => w.DistributorID == item.DistributorID);

                        if (order == null)
                        {
                            Distributor.Add(item);
                        }
                    }
                    return Json(Distributor);
                }
                else
                {
                    var output = context.Distributors
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
    }
}