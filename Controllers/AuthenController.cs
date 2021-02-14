using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class AuthenController : Controller
    {
        // GET: Authen
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account acc)
        {
            if (ModelState.IsValid)
            {
                if (acc.Password.Equals(acc.ConfirmPassword))
                {
                    var userStore = new UserStore<IdentityUser>();
                    var manager = new UserManager<IdentityUser>(userStore);

                    var user = new IdentityUser() { UserName = acc.Username };
                    IdentityResult result = manager.Create(user, acc.Password);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "User Name already exists");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Confirm password is not matched");
                }
            }
            return View(acc);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account acc)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var user = manager.Find(acc.Username, acc.Password);

            if (user != null)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                authenticationManager.SignIn(new AuthenticationProperties { }, userIdentity);
                return RedirectToAction("Index", "Home");
            }
            return View(acc);
        }
        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Authen");
        }
        public static void CreateAccount(string userName, string password, string role)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var user = new IdentityUser(userName);
            manager.Create(user, password);
            manager.AddToRole(user.Id, role);
        }
    }
}