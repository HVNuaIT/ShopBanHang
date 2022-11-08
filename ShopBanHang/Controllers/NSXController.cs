using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Controllers
{
    public class NSXController : Controller
    {
        // GET: NSX
        Shop db = new Shop();
        public ActionResult Index()
        {
            List<NX> listNXS = new List<NX>();

            return View();
        }
    }
}