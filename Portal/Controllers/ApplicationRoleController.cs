using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.Configuration;

namespace Portal.Controllers
{
    [Route("[controller]")]
    public class ApplicationRoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationRoleController(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> AddEditApplicationRole(string id)
        {
            ApplicationRoleViewModel model = new ApplicationRoleViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                    model.Description = applicationRole.Description;
                }
            }
            return PartialView("_AddEditApplicationRole", model);
        }

        [Route("AddEditApplicationRole")]
        [HttpPost]
        public async Task<JsonResult> AddEditApplicationRole([FromBody] ApplicationRoleViewModel model)
        {
            try
            {
               
                ApplicationRole applicationRole =
                new ApplicationRole
                {
                    Name = model.RoleName,
                    Description = model.Description,
                    IPAddress = "",
                    CreatedDate = DateTime.UtcNow
                };
               
                IdentityResult roleRuslt = await roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return Json(true);
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


        [HttpPost]
        public async Task<IActionResult> DeleteApplicationRole(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    IdentityResult roleRuslt = roleManager.DeleteAsync(applicationRole).Result;
                    if (roleRuslt.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }




    }
}