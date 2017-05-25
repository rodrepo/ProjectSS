using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class Inventory : Base
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string ItemModel { get; set; }
        public string SerialNo { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int Quantity { get; set; }

        public int InvNumber { get; set; }
        public string InventoryNumber { get; set; }
    }
}
