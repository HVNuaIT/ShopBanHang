using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
           
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Accout");
            }
            else {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
                return View(giohang);
            }    
        }
     [HttpPost]
        public ActionResult StepEnd()
        {
            //Nhận reqest từ trang index
            string phone = Request.Form["phone"];
            string fullname = Request.Form["fullname"];
            string email = Request.Form["email"];
            string address = Request.Form["address"];
            string note = Request.Form["note"];
            //kiểm tra xem có customer chưa và cập nhật lại
            User newCus = new User();
            var cus = db.Users.FirstOrDefault(p => p.Email.Equals(email));
            /* 
             if (cus != null)
             {
                 //nếu có số điện thoại trong db rồi
                 //cập nhật thông tin và lưu
                 cus.Ten = fullname;
                 cus.Email = email;
                 cus.diaChi = address;
                 db.Entry(cus).State = System.Data.Entity.EntityState.Modified;
                 db.SaveChanges();
             }
             else
             {
                 //nếu chưa có sđt trong db
                 //thêm thông tin và lưu
                 newCus.soDienThoai = phone;
                 newCus.Ten = fullname;
                 newCus.Email = email;
                 newCus.diaChi = address;

                 db.Users.Add(newCus);
                 db.SaveChanges();
             }*/
            //Thêm thông tin vào order và orderdetail
           
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
        
            //thêm order mới
            HoaDon newOrder = new HoaDon();
            newOrder.SoDienThoai = phone;
            newOrder.ghiChu = note;
            newOrder.ngayMuaHang = DateTime.Now;
            newOrder.TenKhach = fullname;
            newOrder.diaChi = address;
            newOrder.Email = email;
            newOrder.maTaiKhoan= cus.maTaiKhoan;
            var donhangnew = db.HoaDons.FirstOrDefault(p => p.Email.Equals(email));
            ChiTietHoaDon newOrdts = new ChiTietHoaDon();
            //thêm details
         
            for (int i = 0; i < giohang.Count; i++)
            {
                newOrdts.maHoaDon = donhangnew.maHoaDon;
                newOrdts.maSanPham = giohang.ElementAtOrDefault(i).SanPhamID;
                newOrdts.soLuong = giohang.ElementAtOrDefault(i).SoLuong;
                newOrdts.donGia = giohang.ElementAtOrDefault(i).DonGia;
                newOrdts.ThanhTien = giohang.ElementAtOrDefault(i).ThanhTien;
                db.ChiTietHoaDons.Add(newOrdts);
                db.SaveChanges();
            }
      
            //gui mail khi dat don hang
            string content = System.IO.File.ReadAllText(Server.MapPath("~/temple/neworder.html"));
            content = content.Replace("{{Ten}}", fullname);
            content = content.Replace("{{SDT}}", phone);
            content = content.Replace("{{Email}}", email);
            content = content.Replace("{{diaChi}}", address);
             content = content.Replace("{{thanhTien}}",newOrdts.ThanhTien.ToString());
            var toEmail = ConfigurationManager.AppSettings["toEmailAddress"].ToString();
            new MailHelper().SendMail(email, "Don hang moi ", content);// gui ve email khach
            new MailHelper().SendMail(toEmail,"Don hang moi ",content);//gui ve email quan tri 

            db.HoaDons.Add(newOrder);
            db.SaveChanges();
            Session["MHD"] = "HD" + newOrder.maHoaDon;
            Session["Phone"] = phone;
            //xoá sạch giỏ hàng
            giohang.Clear();
            return RedirectToAction("HoaDon", "ThanhToan");
        }
        
        public ActionResult HoaDon()
        {
            return View();
        }

    }
}