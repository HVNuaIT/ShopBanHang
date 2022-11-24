using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        Shop db = new Shop();
        public ActionResult Index()
        {
            if (Session["idAdmin"]== null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                
                string a = "Đã thanh toán";
                Session["TongDT"] = db.ChiTietHoaDons.Where(x=>x.TrangThai.Equals(a)).Sum(x=>x.ThanhTien).ToString();
                Session["KhachHang"]= db.Users.Count();
                Session["TongDH"]=db.HoaDons.Count();
                return View();
            }

            
           
        }
    }
}