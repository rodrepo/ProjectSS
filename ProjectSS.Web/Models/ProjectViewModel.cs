using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class ProjectViewModel
    {
        public string CRMName { get; set; }
        public string ProposalTitle { get; set; }

        public string ProjectNo { get; set; }
        public int PJNumber { get; set; }

        public int ProposalId { get; set; }
        public int CRMId { get; set; }
    }
}