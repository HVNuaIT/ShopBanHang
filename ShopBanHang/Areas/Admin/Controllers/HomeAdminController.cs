using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            return View();
        }
        public ActionResult Them()
        {
           

             return View();
        }
        [HttpPost]
        public ActionResult Them(SanPham sanPham)
        {
            try
            {
                
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

          //  return View();
        }
        
    }
}