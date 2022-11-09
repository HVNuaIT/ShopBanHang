using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanHang.Models
{
    public class CartItem
    {
        public string SanPhamID { get; set; }
        public string TenSanPham { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get
            {
                return (SoLuong *DonGia);
            }
        }
        public HoaDon HoaDon { get; set; }
    }
}