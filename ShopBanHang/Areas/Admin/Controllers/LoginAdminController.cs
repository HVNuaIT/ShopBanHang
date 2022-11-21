using ShopBanHang.Models;
using System;
using System.Collections.Generic;
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
                    Session["TenKhachHang"] = check.TaiKhoan;
                    
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
    }
}