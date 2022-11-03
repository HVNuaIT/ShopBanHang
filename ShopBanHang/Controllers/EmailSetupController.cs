using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Controllers
{
    public class EmailSetupController : Controller
    {
        // GET: EmailSetup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ShopBanHang.Models.Gmail model)

        {

            MailMessage mm = new MailMessage(model.From, model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            if (model.File.ContentLength > 0)
            {
                string fileName = Path.GetFileName(model.File.FileName);
                mm.Attachments.Add(new Attachment(model.File.InputStream, fileName));
            }

            mm.IsBodyHtml = false;
            SmtpClient smpt = new SmtpClient();
            smpt.Host = "smtp.gmail.com";
            smpt.Port = 587;
            smpt.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential(model.From, "vkuoutuxfchlsogr");
            smpt.UseDefaultCredentials = true;
            smpt.Credentials = nc;
            smpt.Send(mm);
            ViewBag.Message = "Đã gửi Phản Hồi Cho Admin Thành Công !!!!";


            return View();
        }
    }
}