
using ShopBanHang.Helper;
using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;
using System.Web.WebPages;



//using Configuration = System.Configuration.Configuration;

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
            //User newCus = new User();
            var cus = db.Users.FirstOrDefault(p => p.Email.Equals(email));
        
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
            db.HoaDons.Add(newOrder);
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
                newOrdts.TrangThai = "Chưa Thanh Toán";
              
                var ma = db.SanPhams.Where(x => x.maSanPham == newOrdts.maSanPham).First();
                ma.soLuong -= newOrdts.soLuong;
                db.Entry(ma).State = EntityState.Modified; //THAY DOI TRNAG THAI SO LUONG CUA SAN PHAM
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
      
        public ActionResult Payment()
        {
           

            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            DateTime ngayhientai = DateTime.Parse(TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToShortDateString());
            DateTime ngaygiohientai = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone);
         
        

            PayLib pay = new PayLib();

            string note = Request.Form["note"];
            HoaDon newOrder = new HoaDon();
            newOrder.SoDienThoai = Session["SoDienThoai"].ToString();
            newOrder.ghiChu = note;
            newOrder.ngayMuaHang = ngaygiohientai;
            newOrder.TenKhach = Session["TenKhachHang"].ToString();
            newOrder.diaChi = Session["DiaChi"].ToString();
            newOrder.Email = Session["Email"].ToString();
            newOrder.maTaiKhoan = Convert.ToInt32(Session["idUser"]);
         
            db.HoaDons.Add(newOrder);
            Session["IdDonHang"] = newOrder.maHoaDon;
            var donhangnew = db.HoaDons.FirstOrDefault(p => p.Email.Equals(newOrder.Email));
            

            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];


            pay.AddRequestData("vnp_TxnRef", ngaygiohientai.Ticks.ToString()); //mã hóa đơn

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay' 
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
          

            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            ChiTietHoaDon newOrdts = new ChiTietHoaDon();
            for (int i = 0; i < giohang.Count; i++)
            {

                newOrdts.maSanPham = giohang.ElementAtOrDefault(i).SanPhamID;
                newOrdts.soLuong = giohang.ElementAtOrDefault(i).SoLuong;
                newOrdts.donGia = giohang.ElementAtOrDefault(i).DonGia;
                newOrdts.ThanhTien = giohang.ElementAtOrDefault(i).ThanhTien;
                newOrdts.TrangThai = "Đã thanh toán ";
                newOrdts.maHoaDon =Convert.ToInt32(Session["IdDonHang"]);
               
                var ma = db.SanPhams.Where(x => x.maSanPham == newOrdts.maSanPham).First();
                ma.soLuong -= newOrdts.soLuong;
                db.Entry(ma).State = EntityState.Modified; //THAY DOI TRNAG THAI SO LUONG CUA SAN PHAM
                db.ChiTietHoaDons.Add(newOrdts);

            }
            
            pay.AddRequestData("vnp_Amount", (giohang.FirstOrDefault().ThanhTien * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate",ngaygiohientai.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo","Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            db.SaveChanges();
            giohang.Clear();
            return Redirect(paymentUrl);
        }

       // DateTime.Now.ToString("yyyyMMddHHmmss")
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }









    }
}