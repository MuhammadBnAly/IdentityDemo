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
    [AllowAnonymous]
    public class AccountController : Controller
    {
       [Authorize]
        public ActionResult Logout()
        {
            //HttpCookie c = new HttpCookie("asd");
            //Request.Cookies["asd"]
            IAuthenticationManager authManager= HttpContext.GetOwinContext().Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");

        }

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

                IdentityResult result= manager.Create(myuser,newAccountVM.Password);
                IdentityUserClaim claim1 = new IdentityUserClaim();
                //claim1.ClaimType = "Color";
                //claim1.ClaimValue = "red";
                //myuser.Claims.Add(claim1);
                if (result.Succeeded)//Valid User not Authorize 
                {
                   // manager.AddToRole(myuser.Id, "Admin");
                   
                    IAuthenticationManager authenticationManager =
                        HttpContext.GetOwinContext().Authentication;

                    SignInManager<ApplicationIdentityUser, string> signInManager =
                        new SignInManager<ApplicationIdentityUser, string>(
                            manager, authenticationManager
                            );
                    signInManager.SignIn(myuser, true, true);//Create Cookie ApplicationCookie

                    return RedirectToAction("Index", "Home");
                }else
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault());
                }
            }
            return View(newAccountVM);
        }





        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginUser)
        {
            //Check 
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext("CS");
                UserStore<ApplicationIdentityUser> store
                    = new UserStore<ApplicationIdentityUser>(context);
                UserManager<ApplicationIdentityUser> manager =
                    new UserManager<ApplicationIdentityUser>(store);
                //Check Manager
                ApplicationIdentityUser user=
                    manager.Find(loginUser.UserName, loginUser.Password);
                if (user != null)
                {
                    //cookie
                    IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                    SignInManager<ApplicationIdentityUser, string> signInManager =
                        new SignInManager<ApplicationIdentityUser, string>
                        (manager,authManager);
                    signInManager.SignIn(user, true, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username & password Not Correct");
                }
            }
            return View(loginUser);
        }
    }
}