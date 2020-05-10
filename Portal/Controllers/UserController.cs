using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.Configuration;
using Portal.Models;

namespace Portal.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [Route("AddUser")]
        [HttpPost]
        public async Task<JsonResult> AddUser([FromBody] UserViewModel model)
        {
            try
            {

                ApplicationUser user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ApplicationRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return Json(true);
                        }
                        else
                        {
                            return Json(false);
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
                
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateUser")]
        [HttpPost]
        public async Task<JsonResult> UpdateUser([FromBody] string id, UserViewModel model)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Name = model.Name;
                user.Email = model.Email;

                if (model.Password != "")
                {
                    IdentityResult removePassword = await userManager.RemovePasswordAsync(user); 
                    if(removePassword.Succeeded)
                    {
                        IdentityResult newPassword = await userManager.AddPasswordAsync(user,model.Password);
                    }

                }

                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }

        [Route("UpdatePassword")]
        [HttpPost]
        public async Task<JsonResult> UpdatePassword([FromBody] string id, UserViewModel model)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (model.Password != "")
                {
                    IdentityResult removePassword = await userManager.RemovePasswordAsync(user);
                    if (removePassword.Succeeded)
                    {
                        IdentityResult newPassword = await userManager.AddPasswordAsync(user, model.Password);

                        if (newPassword.Succeeded)
                        {
                            return Json(true);
                        }
                        else
                        {
                            return Json(false);
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<JsonResult> ChangePassword([FromBody] UserManagement userManagement)
        {
            if (!String.IsNullOrEmpty(userManagement.Id))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(userManagement.Id);
                if (applicationUser != null)
                {
                    IdentityResult userRuslt = userManager.ChangePasswordAsync(applicationUser, userManagement.OldPassword, userManagement.NewPassword).Result;
                    if (userRuslt.Succeeded)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(UserViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(model.Id);
                if (applicationUser != null)
                {
                    IdentityResult userRuslt = userManager.DeleteAsync(applicationUser).Result;
                    if (userRuslt.Succeeded)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
            
        }

        [HttpGet]
        public string GenerateUsername(string Name)
        {
            bool isExists = false;
            string username = Regex.Replace(Name, @"\s+", "").ToLower();

            int i = 1; 
            do
            {
                ApplicationUser applicationUser = applicationDbContext.Users
                    .Where(w => w.UserName == username)
                    .FirstOrDefault();

                if (applicationUser != null)
                {
                    username = username + i;
                    isExists = true;
                }
                else
                {
                    isExists = false;
                }
            } while (isExists);

            return username;
        }
        
    }
    
}