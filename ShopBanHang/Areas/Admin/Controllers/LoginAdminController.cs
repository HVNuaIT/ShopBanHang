using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShopBanHang.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: Admin/LoginAdmin
        Shop db = new Shop();
    
        public ActionResult DangNhapAdmin()
        {

       


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhapAdmin(string Email, string pass)
        {
            if (ModelState.IsValid)
            {
                var check = db.Admins.Where(s => s.TaiKhoan.Equals(Email) &&
            s.MatKhau.Equals(pass)).FirstOrDefault();
                ViewData["Message"] = "";
                if (check != null)
                {
                   
                    Session["idAdmin"] = check.maTaiKhoanAdmin;
                    Session["TenAdmin"] = check.TaiKhoan;
                    
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    ViewData["Message"] = "Lỗi Mời Nhập Lại Tài Khoản Và Mật Khẩu";
                    return RedirectToAction("DangNhapAdmin","LoginAdmin");
                }
            }
            return View();
        }
        public ActionResult Thoat()
        {
            Session.Clear();
            return RedirectToAction("DangNhapAdmin", "LoginAdmin");
        }
        public ActionResult ThayDoi()
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ThayDoi(ShopBanHang.Models.Admin admin,string pass,string passnew, string passnews)
        {
             admin = new Models.Admin();
           
            var check = db.Admins.Where(x => x.MatKhau == pass).FirstOrDefault();

            if (check != null && passnew==passnews)
            {

              check.MatKhau = passnew;
                db.Entry(check).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Thay Đổi thành công ";

                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                ViewBag.error = "Thay Đổi Không thành công";

                return RedirectToAction("ThayDoi", "LoginAdmin");
            }
          
        }
    }
}