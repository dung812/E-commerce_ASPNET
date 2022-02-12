using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using CaptchaMvc.HtmlHelpers;

namespace ShopOnline.Controllers
{
    public class UserController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["info"] != null) // Nếu đã đăng nhập rồi thì sửa link vào đăng nhập sẽ điều hướng sang trang chủ
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tk = collection["username"];
            var mk = collection["password"];
            mk = Encryptor.MD5Hash(mk);

            var check = db.Members.SingleOrDefault(model => model.userName == tk && model.password == mk);
            if (ModelState.IsValid)
            {
                if (check == null)
                {
                    ModelState.AddModelError("", "There was a problem logging in. Check your username and password or create an account.");
                }
                else
                {
                    if (!this.IsCaptchaValid(""))
                    {
                        ViewBag.captcha = "Captcha is not valid";
                    }
                    else
                    {
                        Session["info"] = check;
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check = db.Members.Where(model => model.userName == member.userName).FirstOrDefault();

                    if (check != null)
                    {
                        // check username constained in database
                        ModelState.AddModelError("", "There was a problem creating your account. Your username already exists.");
                        return View(member);
                    }
                    else
                    {
                        member.password = Encryptor.MD5Hash(member.password);
                        member.dateOfJoin = DateTime.Now;
                        member.roleId = 3;
                        member.avatar = "~/Content/img/avatar/avatar.jpg";
                        member.status = true;
                        db.Members.Add(member);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            TempData["msgSuccess"] = "Successfully create account!";
                            return RedirectToAction("Login");
                        }
                    }
                }
                return View(member);
            }
            catch(Exception ex)
            {
                TempData["msgFailed"] = "Failed create account! " +ex.Message;
                return RedirectToAction("Login");
            }
        }
        public ActionResult logout()
        {
            Session.Remove("info");
            //Session["info"] = null;
            //Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}