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
        
        }
    }
