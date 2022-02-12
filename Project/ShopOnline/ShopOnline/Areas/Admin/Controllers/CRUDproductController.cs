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

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDproductController : Controller
    {
        menfashionEntities db = new menfashionEntities();

        public ActionResult Index(int? page, string searching)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var products = db.Products.Where(model => model.productName.Contains(searching) || searching == null).OrderByDescending(model => model.dateCreate).Include(model => model.Member).Include(model => model.ProductCategory).ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        //CREATE
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName");
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase uploadFile)
        {
            try
            {
                var productName = product.productName.Trim();
                // Lấy tên sản phẩm để kiểm tra có trùng k
                var check = db.Products.SingleOrDefault(model => model.productName == productName);
                // Xử lí ảnh
                var fileName = Path.GetFileName(uploadFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img/product"), fileName);
                string extension = Path.GetExtension(uploadFile.FileName);

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (check != null)
                    {
                        ModelState.AddModelError("", "Product name already exists");
                    }
                    else
                    {
                        if (uploadFile == null)
                        {
                            ModelState.AddModelError("", "Error while file uploading.");
                        }
                        else
                        {
                            if (product.discount >= 100)
                            {
                                ModelState.AddModelError("", "Discount  percent must less than 100.");
                            }
                            else
                            {
                                product.productName = product.productName.Trim();
                                product.brand = product.brand.Trim();
                                product.image = "~/Content/img/product/" + fileName;
                                product.userName = Session["userNameAdmin"].ToString();
                                product.dateCreate = DateTime.Now;
                                product.status = true;
                                db.Products.Add(product);
                                if (db.SaveChanges() > 0)
                                {
                                    uploadFile.SaveAs(path);
                                    ModelState.Clear();
                                    TempData["msgCreate"] = "Successfully create a new product!";
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid File Type");
                }
                ViewBag.categoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName", product.categoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["msgCreatefailed"] = "Create failed! " + ex.Message;
                return RedirectToAction("Create");
            }
        }

        //EDIT
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Product product = db.Products.Find(id);
            Session["imgPath"] = product.image;
            ViewBag.categoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Product product, HttpPostedFileBase uploadFile, FormCollection collection)
        {
            try
            {
                var descriptiontemp = collection["des"];
                if (ModelState.IsValid)
                {
                    if (uploadFile != null)
                    {
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/img/product"), fileName);

                        product.image = "~/Content/img/product/" + fileName;
                        product.description = descriptiontemp;
                        db.Entry(product).State = EntityState.Modified;
                        string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                        if (db.SaveChanges() > 0)
                        {
                            TempData["msgEdit"] = "Successfully edited product " + product.productName;
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
                        product.description = descriptiontemp;
                        product.image = Session["imgPath"].ToString();
                        db.Entry(product).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            TempData["msgEdit"] = "Successfully edited product has ID: " + product.productId;
                            return RedirectToAction("index");
                        }
                    }
                }
                ViewBag.categoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName", product.categoryId);
                return View(product);
            }
            catch(Exception ex)
            {
                TempData["msgEditFailed"] = "Edit failed! " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        //DELETE
        public ActionResult Delete(int? id)
        {
            try
            {
                var checkInvoice = db.InvoinceDetails.FirstOrDefault(model => model.productId == id);
                // Kiểm tra xem với mã Product có tồn tại trong bảng InvoinceDetail không?
                if (checkInvoice != null) // Nếu có giá trị thì xuất thông báo lỗi
                {
                    TempData["msgDelete"] = "Can't delete this!";
                    return RedirectToAction("Index");
                }
                else
                {
                    Product product = db.Products.Find(id);
                    string currentImg = Request.MapPath(product.image);
                    if (System.IO.File.Exists(currentImg))
                    {
                        System.IO.File.Delete(currentImg);
                    }
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["msgDelete"] = "Can't delete this! " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}