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
using System.Net.Mail;
using System.Text;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class CRUDcontactController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsContact()
        {
            try
            {
                var dsContact = (from i in db.Contacts
                                 orderby i.dateContact descending
                                  select new
                                  {
                                      id = i.id,
                                      name = i.name,
                                      email = i.email,
                                      message = i. message
                                  }).ToList();

                return Json(new { code = 200, dsContact = dsContact, msg = "Successfully get list contact!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Get list contact false: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Contact contact = (from i in db.Contacts
                                   select i).SingleOrDefault(model => model.id == id);
                db.Contacts.Remove(contact);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Successfully delete!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Failed delete! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Reply(int id)
        {
            try
            {
                var detail = (from i in db.Contacts
                              select new
                              {
                                  id = i.id,
                                  name = i.name,
                                  email = i.email
                              }).SingleOrDefault(model => model.id == id);
                //var detail = db.ProductCategories.Where(model => model.categoryId == categoryId).SingleOrDefault();
                return Json(new { code = 200, detail = detail, msg = "Successfully get detail!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Failed get detail!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SendMailToUser(string name,string email, string subject)
        {
            bool result = false;

            result = SendEmail(email, "Phản hồi liên hệ", "<p>Xin chào "+ name +",<br /> "+ subject + " <br />Trân trọng.</p>");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool SendEmail(string toEmail,string subject, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);

                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}