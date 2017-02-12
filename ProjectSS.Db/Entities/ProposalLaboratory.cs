using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class ProposalLaboratory : Base
    {
        public string Name { get; set; }
        public string Parameters { get; set; }
        public int NoOfStations { get; set; }
        public string Replicate { get; set; }
        public int Cost { get; set; }
        public decimal Factor { get; set; }

        public int ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }
    }
}
