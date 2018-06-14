using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManager_data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            //var email = HttpContext.User.Identity.Name;
            var repo = new UserTaskRepository(Properties.Settings.Default.Constr);
            var user = repo.GetByEmail(HttpContext.User.Identity.Name);
            var tasks = repo.GetTasks();
            var hvm = new HomeVM { Tasks = tasks, UserId = user.Id };
            return View(hvm);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            var repo = new UserTaskRepository(Properties.Settings.Default.Constr);            
            repo.Register(user, user.PasswordHash);
            return RedirectToAction("index");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var repo = new UserTaskRepository(Properties.Settings.Default.Constr);
            var user = repo.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("login");
            }
            FormsAuthentication.SetAuthCookie(email, true);
            return Redirect("/home/Index");
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        }

    }
}