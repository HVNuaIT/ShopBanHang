using PagedList;
using PayPal.Api;
using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;

namespace ShopBanHang.Areas.Admin.Controllers
{
    public class SanPhamAdminController : Controller
    {
        // GET: Admin/SanPhamAdmin
        Shop db = new Shop();
        public ActionResult Index(string tim, string search)
        {
           
            var ds = db.SanPhams.ToList();
          
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                if (tim == "NameProduct")
                    return View(db.SanPhams.Where(s => s.tenSanPham.StartsWith(search)).ToList());

                else

                    return View(ds);



            }

            
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                return View();

            }
            
        }
        [HttpPost]
        public ActionResult Create(SanPham product)
        {
            try
            {
               
                db.SanPhams.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var objds = db.SanPhams.Where(x => x.maSanPham == id).FirstOrDefault();
                return View(objds);
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var objds = db.SanPhams.Where(x => x.maSanPham == id).FirstOrDefault();
                return View(objds);

            }
            }
            [HttpPost]
        public ActionResult Delete(string id, SanPham s)
        {
            var objds = db.SanPhams.Where(x => x.maSanPham ==id).FirstOrDefault();
            db.SanPhams.Remove(objds);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {

            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var objds = db.SanPhams.Where(x => x.maSanPham == id).FirstOrDefault();
                return View(objds);

            }
           
        }
        [HttpPost]
        public ActionResult Edit(int id, SanPham product)
        {

           
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }




}
