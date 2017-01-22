using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class CRMRevisionHistory : Base
    {
        public DateTime? DateTime { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }

        public int CRMId { get; set; }
        public virtual CRM CRM { get; set; }
    }
}
