using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LearningWebsite.Business;
using LearningWebsite.Models;

namespace LearningWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClassManager classManager;
        private readonly IUserManager userManager;
        private readonly IUserClassesManager userClassesManager;
        public HomeController(IClassManager classManager, IUserManager userManager, IUserClassesManager userClassesManager)
        {
            this.classManager = classManager;
            this.userManager = userManager;
            this.userClassesManager = userClassesManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.LogIn(loginModel.UserName, loginModel.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "User name and password do not match.");
                }
                else
                {
                    Session["User"] = new LearningWebsite.Models.UserModel
                    {
                        Id = user.Id,
                        Name = user.Name
                    };

                    System.Web.Security.FormsAuthentication.SetAuthCookie(
                        loginModel.UserName, false);

                    return Redirect(returnUrl ?? "~/");
                }
            }

            return View(loginModel);
        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            System.Web.Security.FormsAuthentication.SignOut();

            return Redirect("~/");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Register(registerModel.Email, registerModel.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Existing user, please try a different email.");
                }
                else
                {
                    Session["User"] = new LearningWebsite.Models.UserModel
                    {
                        Id = user.Id,
                        Name = user.Name
                    };

                    System.Web.Security.FormsAuthentication.SetAuthCookie(
                        registerModel.Email, false);

                    return Redirect(returnUrl ?? "~/");
                }
            }

            return View(registerModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ClassList()
        {
            var classes = classManager.Classes
                                        .Select(t => new LearningWebsite.Models.ClassModel(t.Id, t.Name, t.Description, t.Price))
                                        .ToArray();
            var model = new ClassListModel { Classes = classes };
            return View(model);
        }

        [Authorize]
        public ActionResult EnrollClass()
        {
            var classes = classManager.Classes
                                        .Select(t => new LearningWebsite.Models.ClassModel(t.Id, t.Name, t.Description, t.Price))
                                        .ToArray();
            var model = new EnrollClassModel();
            var selectList = new List<SelectListItem>();
            foreach (var course in classes)
            {
                selectList.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.Name
                });
            }
            model.Classes = selectList;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EnrollClass(EnrollClassModel enrollClassModel, string returnUrl)
        {
            var user = (LearningWebsite.Models.UserModel)Session["User"];
            var item = userClassesManager.Add(user.Id, enrollClassModel.ClassId);
            var classes = userClassesManager.GetAll(user.Id)
                .Select(t => new LearningWebsite.Models.ClassModel(t.ClassId, t.Name, t.Description, t.Price))
                .ToArray();
            var model = new ClassListModel { Classes = classes };
            return View("StudentClasses", model);
        }

        [Authorize]
        public ActionResult StudentClasses()
        {
            var user = (LearningWebsite.Models.UserModel)Session["User"];
            var classes = userClassesManager.GetAll(user.Id)
                .Select(t => new LearningWebsite.Models.ClassModel(t.ClassId, t.Name, t.Description, t.Price))
                .ToArray();
            var model = new ClassListModel { Classes = classes };
            return View(model);
        }
    }
}