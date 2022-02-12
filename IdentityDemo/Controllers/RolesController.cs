using IdentityDemo.Models;
using IdentityDemo.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityDemo.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        [HttpGet]

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(RegistrationViewModel newAccountVM)
        {
            if (ModelState.IsValid == true)
            {
                //Save New User Manager=>store=>context=>database
                //1)DEclare UserManager

                //Use inject userstore 
                UserStore<ApplicationIdentityUser> store =
                    new UserStore<ApplicationIdentityUser>
                    (new ApplicationDbContext("CS"));//ApplicationIdenityDbContext

                UserManager<ApplicationIdentityUser> manager =
                    new UserManager<ApplicationIdentityUser>(store);

                //Mapping From VM To Model
                ApplicationIdentityUser myuser = new ApplicationIdentityUser();
                myuser.UserName = newAccountVM.UserName;
                myuser.PasswordHash = newAccountVM.Password;
                myuser.Address = newAccountVM.Address;

                IdentityResult result = manager.Create(myuser, newAccountVM.Password);
                IdentityUserClaim claim1 = new IdentityUserClaim();
                
                if (result.Succeeded)//Valid User not Authorize 
                {
                    manager.AddToRole(myuser.Id, "Admin");

                    IAuthenticationManager authenticationManager =
                        HttpContext.GetOwinContext().Authentication;

                    SignInManager<ApplicationIdentityUser, string> signInManager =
                        new SignInManager<ApplicationIdentityUser, string>(
                            manager, authenticationManager
                            );
                    signInManager.SignIn(myuser, true, true);//Create Cookie ApplicationCookie

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault());
                }
            }
            return View(newAccountVM);
        }

        // GET: Roles
        public ActionResult newRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newRole( string RoleName)
        {
            if (RoleName != null)
            {
                //Save Db
                ApplicationDbContext context =
                    new ApplicationDbContext("CS");
                RoleStore<IdentityRole> store =
                    new RoleStore<IdentityRole>(context);
                RoleManager<IdentityRole> manager =
                    new RoleManager<IdentityRole>(store);
                IdentityRole role = new IdentityRole(RoleName);
                IdentityResult result = manager.Create(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Error2 = result.Errors;
            }
            else {
                ViewBag.Error1 = "Role Cant BE Empty";
            }
            ViewBag.RoleName = RoleName;
            return View();
        }
    }
}