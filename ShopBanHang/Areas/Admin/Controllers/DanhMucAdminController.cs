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
    public class DanhMucAdminController : Controller
    {
        // GET: Admin/DanhMucAdmin
        Shop db  = new Shop();
        public ActionResult Index(string tim, string search)
        {
            if (Session["idAdmin"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "LoginAdmin");
            }
            else
            {
                var ds = db.DanhMucs.ToList();
                if (tim == "DanhMuc")
                    return View(db.DanhMucs.Where(s => s.tenDanhMuc.StartsWith(search)).ToList());

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
        public ActionResult Create(DanhMuc dm)
        {
            try
            {

                db.DanhMucs.Add(dm);
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
                var objds = db.DanhMucs.Where(x => x.maDanhMuc == id).FirstOrDefault();
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
                var objds = db.DanhMucs.Where(x => x.maDanhMuc == id).FirstOrDefault();
                return View(objds);

            }
        }
        [HttpPost]
        public ActionResult Delete(string id, SanPham s)
        {
            var objds = db.DanhMucs.Where(x => x.maDanhMuc == id).FirstOrDefault();
            db.DanhMucs.Remove(objds);
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
                var objds = db.DanhMucs.Where(x => x.maDanhMuc == id).FirstOrDefault();
                return View(objds);

            }

        }
        [HttpPost]
        public ActionResult Edit(int id, DanhMuc product)
        {


            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}