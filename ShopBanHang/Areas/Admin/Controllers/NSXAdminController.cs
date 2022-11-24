using PayPal.Api;
using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanHang.Areas.Admin.Controllers
{
    public class NSXAdminController : Controller
    {
        // GET: Admin/NSXAdmin
        Shop db  = new Shop();
        public ActionResult Index(string tim, string search)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                 var ds = db.NXS.ToList();
                if (tim == "NSX")
                    return View(db.NXS.Where(s => s.TenNSX.StartsWith(search)).ToList());

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
        public ActionResult Create(NX product)
        {
            try
            {

                db.NXS.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var objds = db.NXS.Where(x => x.maNhaSanXuat == id).FirstOrDefault();

                return View(objds);
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var objds = db.NXS.Where(x => x.maNhaSanXuat == id).FirstOrDefault();
                return View(objds);

            }
        }
        [HttpPost]
        public ActionResult Delete(int id, SanPham s)
        {
            var objds = db.NXS.Where(x => x.maNhaSanXuat == id).FirstOrDefault();
            db.NXS.Remove(objds);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var objds = db.NXS.Where(x => x.maNhaSanXuat ==id).FirstOrDefault();
                return View(objds);

            }

        }
        [HttpPost]
        public ActionResult Edit(int id, NX product)
        {


            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}