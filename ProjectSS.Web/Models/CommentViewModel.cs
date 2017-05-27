using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            SubComments = new List<SubCommentViewModel>();
        }

        public int Id { get; set; }
        public string Massage { get; set; }
        public string CommentBy { get; set; }
        public DateTime? CreatedDateConverted { get; set; }

        public List<SubCommentViewModel> SubComments { get; set; }
    }
}