using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        // GET: ChiTietSanPham
        // GET: ChiTietSanPham
        Shop db = new Shop();

        public ActionResult Index(String Id, SanPham product)
        {
            var objPR = db.SanPhams.Where(x => x.maSanPham == Id).FirstOrDefault();
            //  var listCa = db.Categories.ToList();
            //Tim san pham theo danh muc
            //  var listPr = onlineShopEntities.Products.Where(x => x.Id == objPR.CategoryId).ToList();


            ChiTietSanPham s = new ChiTietSanPham();
            s.sanPham = objPR;

            return View(s);
        }
    }
}