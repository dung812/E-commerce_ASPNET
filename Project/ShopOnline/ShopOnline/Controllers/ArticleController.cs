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

namespace ShopOnline.Controllers
{
    public class ArticleController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult ArticleList(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);

            var ListBlog = db.Articles.OrderByDescending(model => model.publicDate).ToPagedList(pageNum, pageSize);
            return View(ListBlog);
        }
        public ActionResult ArticleDetail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var item = db.Articles.Where(model => model.articleId == id).Include(model => model.Member).Single();
            if (item == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(item);
        }
    }
}