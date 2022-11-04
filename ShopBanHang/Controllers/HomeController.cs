using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Shop db = new Shop();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SanPhamMoiNhat()
        {
           

            return View();
        }
    }
}