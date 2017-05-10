using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class ManageCRMViewModel
    {
        public ManageCRMViewModel()
        {
            CRM = new CRMViewModel();
            CallHistorys = new List<CRMCallHistoryModel>();
            EmailHistorys = new List<CRMEmailHistoryModel>();
            RevisionHistorys = new List<CRMRevisionHistoryModel>();
        }

        public CRMViewModel CRM { get; set; }
        public List<CRMCallHistoryModel> CallHistorys { get; set; }
        public List<CRMEmailHistoryModel> EmailHistorys { get; set; }
        public List<CRMRevisionHistoryModel> RevisionHistorys{ get; set; }
    }

    public class CRMCallHistoryModel
    {
        public DateTime? CreatedDateConverted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Remarks { get; set; }
        public string Caller { get; set; }

        public int Id { get; set; }
        public int CRMId { get; set; }

        public string Date
        {
            get { return CreatedDateConverted.Value.ToShortDateString(); }
        }
        public string Time
        {
            get { return CreatedDateConverted.Value.ToShortTimeString(); }
        }
    }

    public class CRMRevisionHistoryModel
    {
        public DateTime? CreatedDateConverted { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string Remarks { get; set; }
        public string UserName { get; set; }

        public int Id { get; set; }
        public int CRMId { get; set; }

        public string Date
        {
            get { return CreatedDateConverted.Value.ToShortDateString(); }
        }
        public string Time
        {
            get { return CreatedDateConverted.Value.ToShortTimeString(); }
        }
    }

    public class CRMEmailHistoryModel
    {
        public DateTime? CreatedDateConverted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Remarks { get; set; }
        public string Sender { get; set; }

        public int Id { get; set; }
        public int CRMId { get; set; }

        public string Date
        {
            get { return CreatedDateConverted.Value.ToShortDateString(); }
        }
        public string Time
        {
            get { return CreatedDateConverted.Value.ToShortTimeString(); }
        }
    }

}
