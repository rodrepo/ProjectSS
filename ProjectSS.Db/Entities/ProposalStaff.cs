using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class ProposalStaff : Base
    {
        public int ManHours { get; set; }
        public int ManMonths { get; set; }
        public decimal Factor { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }
    }
}
