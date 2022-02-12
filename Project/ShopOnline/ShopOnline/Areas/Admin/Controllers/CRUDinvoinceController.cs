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
using Rotativa;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDinvoinceController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var orderlist = db.Invoinces.OrderByDescending(model => model.dateOrder).Include(model => model.Member).Include(model => model.Customer).ToPagedList(pageNumber, pageSize);
            return View(orderlist);
        }
        public ActionResult InvoinceDetail(string invoinceNo)
        {

            var infor = db.Invoinces.Where(model => model.invoinceNo == invoinceNo).Include(model => model.Customer).Include(model => model.Member).FirstOrDefault();
            Session["information"] = infor;
            ViewBag.invoinceNo = invoinceNo;
            var detail = db.InvoinceDetails.Where(model => model.invoinceNo == invoinceNo).Include(model => model.Product).ToList();
            return View(detail);
        }
        public ActionResult Delete(string id)
        {
            List<InvoinceDetail> ctdh = db.InvoinceDetails.Where(model => model.invoinceNo == id).ToList();
            foreach (var i in ctdh)
            {
                db.InvoinceDetails.Remove(i);
            }
            db.SaveChanges();
            Invoince invoince = db.Invoinces.Find(id);
            db.Invoinces.Remove(invoince);
            TempData["DeleteOrder"] = "Successfully delete!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeliverySuccess(string id)
        {
            Invoince invoince = db.Invoinces.Find(id);
            invoince.deliveryDate = DateTime.Now;
            invoince.deliveryStatus = true;
            TempData["Delivery"] = "Order ID " + invoince.invoinceNo + " has been successfully delivered ";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportPDF()
        {
            return new Rotativa.ActionAsPdf("InvoinceDetail");
        }
    }
}