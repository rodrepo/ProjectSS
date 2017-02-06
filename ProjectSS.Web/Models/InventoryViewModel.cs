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
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public UserViewModel User { get; set; }

        public string costodian
        {
            get
            {
                return (User.FirstName + " " + User.MiddleName + " " + User.LastName);
           }
        }
    }
}