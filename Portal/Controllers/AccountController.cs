using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.BL;
using Portal.Configuration;
using Portal.Models;
using Microsoft.EntityFrameworkCore;

namespace Portal.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        DB001Core context = new DB001Core();
        private readonly SignInManager<ApplicationUser> signInManager;
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [Route("ValidateUser")]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> ValidateUser([FromBody] LoginViewModel model)
        {
            try
            {
                UserManagement userManagement = new UserManagement();

                var result = await signInManager.PasswordSignInAsync(model.UserName.Trim(), model.Password.Trim(), model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    userManagement.Status = "Success.";

                    var user = context.UserMasters
                        .Where(u => u.UserName.Trim() == model.UserName.Trim())
                        .Include(p => p.ProfileMaster)
                        .FirstOrDefault();


                    userManagement.UserMaster = user;
                    if (user != null)
                    {
                        var applicationUser = applicationDbContext.Users
                             .Where(w => w.UserName == user.UserName)
                             .FirstOrDefault();

                        if (applicationUser != null)
                        {
                            userManagement.Id = applicationUser.Id;
                        }

                        userManagement.Status = "Success";
                        var menu = context.MenuProfileLinks
                            .Where(w => w.ProfileID == user.ProfileID)
                             .Include(i => i.MenuMaster)
                             .OrderBy(o => o.MenuMaster.SequenceNo)
                             .ToList();

                        List<MenuMaster> listOfMenuMaster = new List<MenuMaster>();
                        foreach (var item in menu)
                        {
                            if (item.MenuMaster.IsActive)
                                listOfMenuMaster.Add(item.MenuMaster);
                        }

                        userManagement.ListOfMenuMaster = listOfMenuMaster;
                    }
                    else
                    {
                        userManagement.Status = "Invalid";
                    }


                    //var menu = context.UserMasters
                    //    .Where(u => u.UserName == _oUserMaster.UserName && u.Password == _oUserMaster.Password)
                    //    .Include(p => p.ProfileMaster)
                    //    .FirstOrDefault();
                }
                else
                {
                    userManagement.Status = "Invalid login attempt.";
                }
                return Json(userManagement);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }


        [Route("AccessDenied")]
        [HttpGet]
        public IActionResult AccessDenied(string Url)
        {
            return RedirectToAction("SignOff", "Account");
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string Url)
        {
            return RedirectToAction("SignOff", "Account");
        }

        [Route("SignOff")]
        [HttpGet]
        public async Task<IActionResult> SignOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}