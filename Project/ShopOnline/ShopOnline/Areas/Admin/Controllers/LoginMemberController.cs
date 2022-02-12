using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using System.Net;
using System.Data.Entity;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class LoginMemberController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tk = collection["username"];
            var mk = collection["password"];
            mk = Encryptor.MD5Hash(mk);

            var notMember = db.Members.Where(model => model.roleId == 3).SingleOrDefault(model => model.userName == tk && model.password == mk);
            var check = db.Members.Where(model => model.roleId == 1 || model.roleId == 2).SingleOrDefault(model => model.userName == tk && model.password == mk);
            if (ModelState.IsValid)
            {
                if (check == null)
                {
                    if (notMember != null)
                    {
                        ModelState.AddModelError("", "Role not invalid.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was a problem logging in. Check your username and password or create an account.");
                    }
                }

                else
                {
                    if (!this.IsCaptchaValid(""))
                    {
                        ViewBag.captcha = "Captcha is not valid";
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(check.lastName, false);
                        Session["userNameAdmin"] = check.userName;
                        Session["infoAdmin"] = check;
                        return RedirectToAction("Index", "DashBoard");
                    }
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}