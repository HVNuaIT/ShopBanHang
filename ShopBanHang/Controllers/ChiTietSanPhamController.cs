using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShopBanHang.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        
        Shop db = new Shop();
      
        public ActionResult ChiTiet(String Id)
        {
            Session["id"] = Id;
         

            return View(db.SanPhams.SingleOrDefault(p => p.maSanPham.Equals(Id)));
        }
        public ActionResult BinhLuan()
        {
            var ds = db.BinhLuans.ToList();
            ViewBag.Mess = "Bình Luận thành công";
            return View(ds);
        }
      [HttpPost]        
        public ActionResult BinhLuan(String sao,String NoiDung)
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Accout");
            }
            else { 
                
            BinhLuan bl = new BinhLuan();
              
            bl.SaoBinhLuan = Convert.ToInt32(sao);
            bl.noiDung= NoiDung;
            bl.maTaiKhoan = Convert.ToInt32(Session["idUser"]);
            bl.maSanPham = Session["id"].ToString();
            bl.Ngay = DateTime.Now;
            db.BinhLuans.Add(bl);
            db.SaveChanges();
                return RedirectToAction("BinhLuan","ChiTietSanPham");
            }
        }
    }
}