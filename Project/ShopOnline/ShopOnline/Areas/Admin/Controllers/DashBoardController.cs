using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index()
        {
            /*---------------------------------------THỐNG KÊ------------------------------*/
            // Lấy ra tổng số tiền các đơn hàng - lợi nhuận
            ViewBag.profit = db.InvoinceDetails.Sum(model => model.totalPrice);
            // Lấy ra tổng số lượng các user khách hàng đang có trong database - tổng số tài khoản hiện tại
            ViewBag.users = db.Members.Where(model => model.roleId == 3).Count();
            // Lấy ra tổng số các đơn đặt hàng đang có trong database - tổng số đơn hàng 
            ViewBag.orders = db.Invoinces.Count();
            // Lấy số lượng người truy cập từ Application đã tạo
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            return View();
        }
        public PartialViewResult Chart()
        {

            // Danh thu sản phẩm cao nhất, thu được:
            /*(tên sản phẩm - hình ảnh - tổng danh thu dựa trên đơn hàng)*/
            var getTopProduct = (db.Products.Join(db.InvoinceDetails,
                                sanPham => sanPham.productId,
                                ctDonHang => ctDonHang.productId,
                                (sanPham, ctDonHang) => new {
                                    tensanpham = sanPham.productName,
                                    hinh = sanPham.image,
                                    gia = ctDonHang.quanlityProduct * sanPham.price,
                                })).ToList();
            var test = getTopProduct;

            // Đếm số lượng sản phẩm trong từng loại sản phẩm, thu được: (Đã đổ dữ liệu vào pie-chart)
            /*(tên loại sản phẩm - số lượng sản phẩm thuộc loại đó)*/
            var countProductOfCategory = (from c in db.ProductCategories
                                          join p in db.Products on c.categoryId equals p.categoryId into temp1
                                          from temp2 in temp1.DefaultIfEmpty()
                                          group temp2 by c.categoryName into grouped
                                          select new
                                          {
                                              ten = grouped.Key,
                                              count = grouped.Count(t => t.productId != null)
                                          }).ToList();
            var categoriesData = countProductOfCategory;

            List<string> categories = new List<string>();
            List<string> categoriesCount = new List<string>();
            foreach (var i in categoriesData)
            {
                categories.Add(i.ten);
            }
            foreach (var i in categoriesData)
            {
                categoriesCount.Add(i.count.ToString());
            }
            ViewBag.categories = categories.ToList();
            ViewBag.categoriesCount = categoriesCount.ToList();

            // Thống kê doanh thu bán hàng theo ngày, thu được: (Đã đổ dữ liệu vào Line-chart)
            /*(ngày bán - doanh thu)*/
            
            var doanhthu = db.vDoanhThuTheoNgays.OrderByDescending(model => model.dateOrder).Take(7).ToList(); // Sắp xếp giảm dần theo ngày và lấy 7 ngày gần nhất
            // Đảo ngược mảng để hiển thị ngày theo thứ tự tăng dần
            doanhthu.Reverse();

            List<string> date = new List<string>();
            List<int> income = new List<int>();
            foreach (var i in doanhthu)
            {
                date.Add(i.dateOrder);
            }
            foreach (var i in doanhthu)
            {
                income.Add((int)i.income);
            }
            ViewBag.date = date.ToList();
            ViewBag.income = income.ToList();
            // Chart
            return PartialView();
        }
        public PartialViewResult InvoinceList() // Danh sách hóa đơn trong ngày
        {
            var list = db.vHoaDonTrongNgays.OrderByDescending(model => model.dateOrder).ToList();
            return PartialView(list);
        }
        public ActionResult TrackingCovid()
        {
            return View();
        }        
        public ActionResult Chat()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditProfie(string userName)
        {
            Member member = db.Members.Find(userName);
            Session["imgPath"] = member.avatar;
            return View(member);
        }
        [HttpPost]
        public ActionResult EditProfie(Member member, HttpPostedFileBase uploadFile)
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

                        string oldImgPath = Request.MapPath(Session["imgPath"].ToString()); // Lấy đường dẫn ảnh (absolute path)
                        var avatarName = Session["imgPath"].ToString(); // Lấy đường dẫn ảnh (relative path)
                        var checkAvatart = db.Members.Where(model => model.avatar == avatarName).ToList(); // Kiểm tra ảnh có trùng với avatar của member nào không

                        if (db.SaveChanges() > 0)
                        {
                            uploadFile.SaveAs(path);
                            if (System.IO.File.Exists(oldImgPath) && checkAvatart.Count < 2) // Nếu tồn tại hình trong folder và không member nào có hình này thì xóa ra khỏi folder
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                            var info = db.Members.Where(model => model.userName == member.userName).SingleOrDefault();
                            Session["infoAdmin"] = info; // Lấy thông tin mới của member lưu lại vào session hiển thị
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        member.avatar = Session["imgPath"].ToString();
                        db.Entry(member).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            var info = db.Members.Where(model => model.userName == member.userName).SingleOrDefault();
                            Session["infoAdmin"] = info;
                            return RedirectToAction("Index");
                        }
                    }
                }
                ViewBag.roleId = new SelectList(db.Roles, "roleId", "roleName", member.roleId);
                return View(member);
            }
            catch (Exception ex)
            {
                TempData["msgEditProfieFailed"] = "Edit failed! " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult ChangePassword(string userName)
        {
            Member member = db.Members.Find(userName);
            return View(member);
        }
        [HttpPost]
        public ActionResult ChangePassword(Member member, FormCollection collection)
        {
            try
            {
                var CurrentPassword = collection["CurPassword"]; // Mật khẩu hiện tại
                var NewPassword = collection["NewPassword"]; // Mật khẩu mới
                //var ConfirmPassword = collection["Confirm"];

                CurrentPassword = Encryptor.MD5Hash(CurrentPassword.Trim());
                NewPassword = Encryptor.MD5Hash(NewPassword.Trim());

                var check = db.Members.Where(model => model.password == CurrentPassword && model.userName == member.userName).FirstOrDefault();
                if (check!= null)
                {
                    check.password = NewPassword;
                    db.SaveChanges();
                    TempData["msgChangePassword"] = "Successfully change password!";
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect your password!");
                    return View(member);
                }
                return View(member);
            }
            catch(Exception ex)
            {
                TempData["msgChangePasswordFailed"] = "Edit failed! " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}