using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Common;
using Portal.Configuration;
using Portal.Models;

namespace Portal.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    [Route("[controller]")]
    public class DistributorController : Controller
    {
        DB001Core context = new DB001Core();
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public DistributorController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Add")]
        [HttpPost]
        public async Task<bool> Add([FromBody] Distributor _oDistributor)
        {

            try
            {
                CommonLogic clsCommon = new CommonLogic();
                _oDistributor.DistributorCode = clsCommon.GenerateDistributorCode();

                dynamic output;
                using (var context = new DB001Core())
                {
                    _oDistributor.CreatedOn = DateTime.Now;
                    _oDistributor.ModifiedOn = DateTime.Now;

                    context.Distributors.Add(_oDistributor);
                    output = context.SaveChanges();
                }
                if (output > 0)
                {
                    using (var context = new DB001Core())
                    {
                        #region Add AspNet User
                        var role = context.ProfileMasters
                                              .Where(w => w.ProfileName == "Distributor")
                                              .FirstOrDefault();

                        ApplicationRole applicationRole = await roleManager.FindByNameAsync(role.ProfileName);

                        if (applicationRole != null)
                        {
                            UserController userController = new UserController(userManager, roleManager);

                            string username = userController.GenerateUsername(_oDistributor.DistributorName);
                            UserViewModel userViewModel = new UserViewModel
                            {
                                Name = _oDistributor.DistributorName,
                                UserName = username,
                                Password = "1234",
                                ApplicationRoleId = applicationRole.Id
                            };

                            
                            var appOutput = await userController.AddUser(userViewModel);

                            if (Convert.ToBoolean(appOutput.Value))
                            {

                                UserMaster userMaster = new UserMaster
                                {
                                    Name = _oDistributor.DistributorName,
                                    UserName = username,
                                    Password = null,
                                    ProfileID = role.ProfileID,
                                    DistributorID = _oDistributor.DistributorID,
                                    MobileNo = _oDistributor.MobileNo,
                                    IsActive = true
                                };

                                UserManagementController user = new UserManagementController();
                                output = user.Add(userMaster);
                            }
                        }

                        #endregion
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
        public bool Update([FromBody] Distributor _oDistributor)
        {
            try
            {
                int output;
                using (var context = new DB001Core())
                {
                    var input = context.Distributors
                                   .Where(w => w.DistributorID == _oDistributor.DistributorID)
                                   .FirstOrDefault();

                    input.DistributorName = _oDistributor.DistributorName;
                    input.Email = _oDistributor.Email;
                    input.MobileNo = _oDistributor.MobileNo;
                    input.Address = _oDistributor.Address;
                    input.IsActive = _oDistributor.IsActive;
                    input.CityID = _oDistributor.CityID;
                    input.DistrictID = _oDistributor.DistrictID;
                    input.StateID = _oDistributor.StateID;
                    input.ModifiedOn = DateTime.Now;

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
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetList()
        {

            try
            {
                var output = context.Distributors
                    .OrderByDescending(o => o.DistributorID)
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
        public JsonResult Get(long distributorId)
        {
            try
            {
                var output = context.Distributors
                                     .Where(s => s.DistributorID == distributorId)
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