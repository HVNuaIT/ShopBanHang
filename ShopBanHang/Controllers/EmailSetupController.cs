using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBanHang.Models;
using System.Configuration;
using System.Security.Policy;

namespace ShopBanHang.Controllers
{
    public class EmailSetupController : Controller
    {
        // GET: EmailSetup
        Shop db  = new Shop();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ShopBanHang.Models.LienHe model)

        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            MailMessage mm = new MailMessage(model.Email,fromEmailAddress);
 
            string content = System.IO.File.ReadAllText(Server.MapPath("~/temple/LienHe.html"));
            content = content.Replace("{{Ten}}", model.TenKhachHang);
            content = content.Replace("{{SDT}}", model.SDT);
            content = content.Replace("{{Messager}}", model.LoiNhan);
            content = content.Replace("{{mail}}", model.Email);
            var toEmail = ConfigurationManager.AppSettings["toEmailAddress"].ToString();
            new MailHelper().SendMail(toEmail, "Thông tin Liên hệ từ khách hàng  ", content);//gui ve email quan tri 

            db.Configuration.ValidateOnSaveEnabled = false;
            db.LienHes.Add(model);
            db.SaveChanges();
            ViewBag.Message = "Đã gửi Phản Hồi Cho Admin Thành Công !!!!";
            return View();
        }
    }
}