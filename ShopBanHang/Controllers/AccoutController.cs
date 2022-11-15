using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.Entity;
using PayPal;

namespace ShopBanHang.Controllers
{
    public class AccoutController : Controller
    {
        // GET: Accout
        Shop db = new Shop();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XacThuc(string Email,string pass)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.Where(s => s.Email.Equals(Email) &&
            s.matKhau.Equals(pass) && s.xacThucEmail==true).ToList();
              
                if (check!=null)
                {
                    Session["Email"] = check.FirstOrDefault().Email;
                    Session["idUser"] = check.FirstOrDefault().maTaiKhoan;
                    Session["TenKhachHang"] = check.FirstOrDefault().Ten;
                    Session["DiaChi"] = check.FirstOrDefault().diaChi;
                    Session["SoDienThoai"] = check.FirstOrDefault().soDienThoai;
                    Session["gioitinh"] = check.FirstOrDefault().gioiTinh;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.err = "Tài Khoản Chưa Đăng kí";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XacThucDangKi(string Email,string pass,string diaChi,string ten,int sdt,string gioitinh)
        {
           

            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email.Equals(Email));


                if (check == null)
                {

                   db.Configuration.ValidateOnSaveEnabled = false;
                   User tk = new User()
                    {
                        Email = Email,
                    matKhau = pass,
                    diaChi = diaChi,
                    Ten = ten,
                    soDienThoai = sdt.ToString(),
                    gioiTinh = gioitinh,
                };
                    tk.ActivationCode = Guid.NewGuid();


                    db.Users.Add(tk);

                    db.SaveChanges();
                    SendVerificationLinkEmail(tk.Email, tk.ActivationCode.ToString());
                    //message = "Đăng Kí thành Công .Links Xác thực tài khoản " +
                      //  " has been sent to your email id:" + tk.Email;
                  
                    ViewBag.Message = "Đăng Kí thành công Bạn Hãy Kiểm Tra Email Để Thực Hiện Đăng Nhập Nhé !!! "+tk.Email;
                    //return RedirectToAction("DangKi", "Accout");
                    return View("DangKi");
                }
                else
                {
                    ViewBag.error = "Đăng Kí Không thành Công ";
                    return View("DangKi");
                }

            }
            return View();
        }
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (Shop dc = new Shop())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.xacThucEmail = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/Accout/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("namchibi18@gmail.com", "ShopVP");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "richuonmbfsbdign"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>Cảm ơn quý khách đã đăng kí tài khoản " +
                " Thành Công.Vui lòng click vào Link để thực hiện việc xác thực tài khoản và đăng nhập" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }


        public ActionResult ThayDoiThongTin()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Accout");
            }
            else
            {
                return View();
            }

           
        }
        [HttpPost]
        public ActionResult Xacthucthaydoi( string diaChi, string ten, int sdt, string gioitinh)
        {
            
          User tk = new User();
            tk.Email = Session["Email"].ToString(); 
            var check = db.Users.Where(x => x.Email == tk.Email).FirstOrDefault();
            if (check != null) {
                
                check.Ten = ten;
                check.diaChi = diaChi;
                check.gioiTinh = gioitinh;
                check.soDienThoai = sdt.ToString();
                db.Entry(check).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Thay Đổi thành công ";
               
                return RedirectToAction("Login","Accout");
            }
            else
            {
                ViewBag.error = "Thay Đổi Không thành công";
                
                return RedirectToAction("ThayDoiThongTin", "Accout");
            }
           
        }
    }
}