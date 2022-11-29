
using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ShopBanHang.Areas.Admin.Controllers
{
    public class DonHangAdminController : Controller
    {
        // GET: Admin/DonHangAdmin
        Shop db = new Shop();
        public ActionResult Index(int? page)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
            
                var dsDH = db.ChiTietHoaDons.ToList();
                 return  View(dsDH);
              
            }
        }

            [HttpGet]
            public ActionResult Delete(int id)
            {
                if (Session["idAdmin"] == null)
                {
                    return RedirectToAction("DangNhapAdmin", "LoginAdmin");
                }
            else
            {
                var objds = db.ChiTietHoaDons.Where(x => x.maHoaDon == id).FirstOrDefault();


                return View(objds);
                }
            }
            [HttpPost]
            public ActionResult Delete(int id, ChiTietHoaDon s)
            {
                var objds = db.HoaDons.Where(x => x.maHoaDon ==id ).FirstOrDefault();
            var ds = db.ChiTietHoaDons.Where(a => a.maHoaDon == id).FirstOrDefault();
            db.HoaDons.Remove(objds);
            db.ChiTietHoaDons.Remove(ds);
            db.SaveChanges();
                return RedirectToAction("Index");
            }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                
                var objds = db.HoaDons.Where(x => x.maHoaDon == id).FirstOrDefault();
                return View(objds);
            }
        }



    }
}