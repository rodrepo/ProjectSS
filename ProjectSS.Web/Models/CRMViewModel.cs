using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class CRMViewModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Number { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string Municipality { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string Email { get; set; }
        public string OfficeNo { get; set; }
        public string FaxNo { get; set; }
        public string MobileNo { get; set; }
    }
}