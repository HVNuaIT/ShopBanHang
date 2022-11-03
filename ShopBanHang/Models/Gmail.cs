using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanHang.Models
{
    public class Gmail
    {
        public string To { get; set; }
        public string From{ get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Phone { get; set; }   
        public string Name { get; set; }
        public string Password { get; set; }
        public HttpPostedFileBase File { get; set; } 
    }
}