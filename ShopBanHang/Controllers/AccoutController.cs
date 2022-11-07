using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult XacThuc(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var check = db.TaiKhoans.Where(s => s.Email.Equals(taiKhoan.Email) &&
            s.matKhau.Equals(taiKhoan.matKhau)).ToList();
                if (check.Count > 0)
                {


                    Session["Email"] = check.FirstOrDefault().Email;
                    Session["idUser"] = check.FirstOrDefault().maTaiKhoan;

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
        [HttpGet]
        public ActionResult XacThucDangKi(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var check = db.TaiKhoans.FirstOrDefault(s => s.Email == taiKhoan.Email);


                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.TaiKhoans.Add(taiKhoan);

                    db.SaveChanges();


                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }

            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


    }
}