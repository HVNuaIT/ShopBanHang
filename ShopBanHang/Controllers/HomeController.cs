using ShopBanHang.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Razor.Parser.SyntaxTree;
using System.Xml.Linq;
using PayPal.Api;

namespace ShopBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Shop db = new Shop();
        public ActionResult Index(string name)
        {
           

                return View();
            
        }
        public ActionResult SanPhamMoiNhat(int? page)
        {
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            var links = (from l in db.SanPhams
                         select l).OrderBy(x => x.maSanPham);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 3;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));

           // return View();
        }

        public ActionResult SearchByName(string name, int? page)
        {
            ViewBag.search = name;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(db.SanPhams.Where(p => p.tenSanPham.Contains(name)).OrderByDescending(x => x.tenSanPham).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchByDanhMuc(string name, int? page)
        {
            ViewBag.search = name;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(db.DanhMucs.Where(p => p.tenDanhMuc.Contains(name)).OrderByDescending(x => x.tenDanhMuc).ToPagedList(pageNumber, pageSize));
        }

       
    }
}