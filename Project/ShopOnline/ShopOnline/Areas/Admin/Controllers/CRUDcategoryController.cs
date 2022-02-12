using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDcategoryController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsCategory()
        {
            try
            {
                var dsCategory = (from i in db.ProductCategories
                                select new
                                {
                                   categoryId = i.categoryId,
                                   categoryName = i.categoryName
                                }).ToList();

                return Json(new { code = 200, dsCategory = dsCategory, msg = "Successfully get list Category!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Get list Category false: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult AddCategory(string categoryName)
        {
            try
            {
                var category = new ProductCategory();
                category.categoryName = categoryName;

                db.ProductCategories.Add(category);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Successfully added a new product category!!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Error: "+ ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int categoryId)
        {
            try
            {
                var detail = (from i in db.ProductCategories
                              select new
                              {
                                  categoryId = i.categoryId,
                                  categoryName = i.categoryName
                              }).SingleOrDefault(model => model.categoryId == categoryId);
                //var detail = db.ProductCategories.Where(model => model.categoryId == categoryId).SingleOrDefault();
                return Json(new { code = 200, detail = detail, msg = "Successfully get detail!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Failed get detail!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(int categoryId, string categoryName)
        {
            try
            {
                var category = (from i in db.ProductCategories
                                select i).SingleOrDefault(model => model.categoryId == categoryId);
                category.categoryName = (string)categoryName;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Successfully update!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Faily update" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int categoryId)
        {
            try
            {
                ProductCategory category = (from i in db.ProductCategories
                                select i).SingleOrDefault(model => model.categoryId == categoryId);
                db.ProductCategories.Remove(category);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Successfully delete!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Failed delete! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}