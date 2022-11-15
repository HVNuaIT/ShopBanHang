using PagedList;
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
          

            return View();
        }

        public ActionResult ProductsByPdc(string id, int? page)
        {
            ViewBag.pdcName = db.NXS.SingleOrDefault(c => c.maSanPham == id).maSanPham;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(db.SanPhams.Where(p => p.maSanPham == id).OrderByDescending(x => x.maSanPham).ToPagedList(pageNumber, pageSize));
        }
    }
}