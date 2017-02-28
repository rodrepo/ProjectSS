using ProjectSS.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            ProposalModel = new ProposalViewModel();
        }

        public int Id { get; set; }
        public string CRMName { get; set; }
        public string ProposalTitle { get; set; }

        public string ProjectNo { get; set; }
        public int PJNumber { get; set; }

        public int ProposalId { get; set; }
        public int CRMId { get; set; }

        public string BDName { get; set; }
        public string THName { get; set; }
        public string TSName { get; set; }

        public ProposalViewModel ProposalModel { get; set; }
    }
}