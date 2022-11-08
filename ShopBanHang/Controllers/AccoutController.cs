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
        public ActionResult XacThuc(string Email,string pass)
        {
            if (ModelState.IsValid)
            {
                var check = db.TaiKhoans.Where(s => s.Email.Equals(Email) &&
            s.matKhau.Equals(pass)).ToList();
                if (check.Count > 0)
                {


                    Session["Email"] = check.FirstOrDefault().Email;
                    Session["idUser"] = check.FirstOrDefault().maTaiKhoan;
                    Session["TenKhachHang"] = check.FirstOrDefault().Ten;

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
                var check = db.TaiKhoans.FirstOrDefault(s => s.Email.Equals(Email));


                if (check == null)
                {

                   db.Configuration.ValidateOnSaveEnabled = false;
                    TaiKhoan tk = new TaiKhoan()
                    {
                        Email = Email,
                    matKhau = pass,
                    diaChi = diaChi,
                    Ten = ten,
                    soDienThoai = sdt.ToString(),
                    gioiTinh = gioitinh,
                };
                    

                    db.TaiKhoans.Add(tk);

                    db.SaveChanges();

                    ViewBag.Message = "Đăng Kí thành công Bạn có thể đăng nhập với tài khoản này";
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

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


    }
}