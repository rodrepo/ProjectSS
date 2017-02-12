using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class ProposalCommission : Base
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal Cost { get; set; }

        public int ProposalId { get; set; }
        public virtual Proposal Proposal{ get; set; }
    }
}