using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Areas.Admin.Controllers
{
    public class DonHangAdminController : Controller
    {
        // GET: Admin/DonHangAdmin
        Shop db = new Shop();
        public ActionResult Index()
        {

            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var dsct = db.ChiTietHoaDons.ToList();
                var dsDH = db.HoaDons.ToList();
                var table  = dsct.Where(b=>dsDH.Any(a=>a.maHoaDon==b.maHoaDon)).ToList();

               
                    return View(table);

               

            }
        }
    }
}