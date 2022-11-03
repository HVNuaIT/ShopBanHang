using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Controllers
{
    public class ThanhToanController : Controller
    {
        // GET: ThanhToan
        Shop db = new Shop();


        // GET: ThanhToan
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);

        }


        public ActionResult HoaDon()
        {
            return View();
        }
    }
}