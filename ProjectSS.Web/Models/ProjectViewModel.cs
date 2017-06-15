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
        public int TableNumber { get; set; }
        public int Id { get; set; }
        public string CRMName { get; set; }
        public string ProposalTitle { get; set; }

        public string ProjectNo { get; set; }
        public int PJNumber { get; set; }

        public bool IsClosed { get; set; }

        public int ProposalId { get; set; }
        public int CRMId { get; set; }

        public string BDName { get; set; }
        public string THName { get; set; }
        public string TSName { get; set; }

        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }
        public string CreatedBy { get; set; }

        // For Progress bar
        public string Progress
        {
            get
            {
                if( RemainingBudget > 0 && Budget > 0)
                {
                    return (string.Format("{0:0}", RemainingBudget / Budget * 100).ToString());
                }
                else
                {
                    return ("0");
                }
            }
        }

        public string ProgressRoundOf
        {
            get
            {
                if (RemainingBudget > 0 && Budget > 0)
                {
                    if (Progress != "100")
                    {
                        return ((10 - int.Parse(Progress) % 10) + int.Parse(Progress)).ToString();
                    }
                    else
                    {
                        return ("100");
                    }
                }
                else
                {
                    return ("0");
                }
            }
        }

        public string Color
        {
            get
            {
                if (int.Parse(Progress) > 70)
                {
                    return ("success");
                }
                else if (int.Parse(Progress) > 40 && int.Parse(Progress) < 69)
                {
                    return ("Warning");
                }
                else
                {
                    return ("danger");
                }
            }
        }

        public ProposalViewModel ProposalModel { get; set; }
    }
}