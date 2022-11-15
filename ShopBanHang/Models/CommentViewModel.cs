using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanHang.Models
{
    public class CommentViewModel
    {
        public long ID { get; set; }
        public string CommentMsg { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
        public SanPham sp { get; set; }
        public User user { get; set; }
        public long ParentID { get; set; }
        public int Rate { get; set; }
    }
}