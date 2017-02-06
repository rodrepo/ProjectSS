using ProjectSS.Web.Models.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string ItemModel { get; set; }
        public string SerialNo { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Costodian { get; set; }
    }
}