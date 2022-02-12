using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDmemberController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index(int? page, string searching)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var member = db.Members.Where(model => model.userName.Contains(searching) || searching == null).OrderByDescending(model => model.dateOfJoin).Include(model => model.Role).ToPagedList(pageNumber, pageSize);
            return View(member);
        }
        //CREATE
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.roleId = new SelectList(db.Roles, "roleId", "roleName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Member member, HttpPostedFileBase uploadFile)
        {
            try
            {
                var username = member.userName.Trim();
                // Lấy tên đăng nhập đã nhập vào kiểm tra có trùng 
                var check = db.Members.SingleOrDefault(model => model.userName == username);
                // Xử lí ảnh
                var fileName = Path.GetFileName(uploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img/avatar"), fileName);
                string extension = Path.GetExtension(uploadFile.FileName);

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (check != null)
                    {
                        ModelState.AddModelError("", "Username already exists");
                    }
                    else
                    {
                        if (uploadFile == null)
                        {
                            ModelState.AddModelError("", "Error while file uploading.");
                        }
                        else
                        {
                            member.firstName = member.firstName.Trim();
                            member.lastName = member.lastName.Trim();
                            member.email = member.email.Trim();
                            member.address = member.address.Trim();
                            member.phone = member.phone.Trim();
                            member.identityNumber = member.identityNumber.Trim();
                            member.avatar = "~/Content/img/avatar/" + fileName;
                            member.dateOfJoin = DateTime.Now;
                            member.status = true;
                            member.password = Encryptor.MD5Hash(member.password);
                            db.Members.Add(member);
                            if (db.SaveChanges() > 0)
                            {
                                uploadFile.SaveAs(path);
                                ModelState.Clear();
                                TempData["msgCreate"] = "Successfully create a new member!";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid File Type");
                }

                ViewBag.roleId = new SelectList(db.Roles, "roleId", "roleName", member.roleId);
                return View(member);
            }
            catch(Exception ex)
            {
                TempData["msgCreatefailed"] = "Create failed! " + ex.Message;
                return RedirectToAction("Create");
            }
        }

        // EDIT
        [HttpGet]
        public ActionResult Edit(string userName)
        {
            Member member = db.Members.Find(userName);
            Session["imgPath"] = member.avatar;
            ViewBag.roleId = new SelectList(db.Roles, "roleId", "roleName", member.roleId);
            return View(member);
        }
        [HttpPost]
        public ActionResult Edit(Member member, HttpPostedFileBase uploadFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadFile != null)
                    {
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/img/avatar"), fileName);
                        member.avatar = "~/Content/img/avatar/" + fileName;
                        db.Entry(member).State = EntityState.Modified;
                        string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                        if (db.SaveChanges() > 0)
                        {
                            TempData["msgEdit"] = "Successfully edited!";
                            uploadFile.SaveAs(path);
                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        member.avatar = Session["imgPath"].ToString();
                        db.Entry(member).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            TempData["msgEdit"] = "Successfully edited!";
                            return RedirectToAction("index");
                        }
                    }
                }
                ViewBag.roleId = new SelectList(db.Roles, "roleId", "roleName", member.roleId);
                return View(member);
            }
            catch(Exception ex)
            {
                TempData["msgEditFailed"] = "Edit failed! " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // DELETE
        public ActionResult Delete(string userName)
        {
            try
            {
                Member member = db.Members.Find(userName);
                var checkArticle = db.Articles.FirstOrDefault(model => model.userName == userName);
                var checkInvoice = db.Invoinces.FirstOrDefault(model => model.userName == userName);
                var checkProduct = db.Products.FirstOrDefault(model => model.userName == userName);
                string currentImg = Request.MapPath(member.avatar);
                // Kiểm tra xem  tài khoản có trong bảng Article, bảng Invoice hoặc bảng Product không
                if (checkArticle != null || checkInvoice != null || checkProduct != null) // Nếu có 1 trong 3 giá trị thì không cho xóa
                {
                    TempData["msgDeleteFailed"] = "Can't delete this! ";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Kiểm tra xem tài khoản đăng nhập hiện tại có trùng với tài khoản xóa hay không
                    if (Session["userNameAdmin"].ToString() == member.userName) // Nếu trùng thì xóa tài khoản và đăng xuất, chuyển về trang đăng nhập
                    {
                        var avatarName = member.avatar.ToString(); // Lấy đường dẫn ảnh (relative path)
                        var checkAvatart = db.Members.Where(model => model.avatar == avatarName).ToList(); // Kiểm tra ảnh có trùng với avatar của member nào không
                        if (System.IO.File.Exists(currentImg) && checkAvatart.Count < 2)
                        {
                            System.IO.File.Delete(currentImg);
                        }
                        db.Members.Remove(member);
                        db.SaveChanges();
                        FormsAuthentication.SignOut();
                        return RedirectToAction("Login", "LoginMember");
                    }
                    else
                    {
                        var avatarName = member.avatar.ToString(); // Lấy đường dẫn ảnh (relative path)
                        var checkAvatart = db.Members.Where(model => model.avatar == avatarName).ToList(); // Kiểm tra ảnh có trùng với avatar của member nào không
                        if (System.IO.File.Exists(currentImg) && checkAvatart.Count < 2)
                        {
                            System.IO.File.Delete(currentImg);
                        }
                        db.Members.Remove(member);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msgDeleteFailed"] = "Can't delete this! " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}