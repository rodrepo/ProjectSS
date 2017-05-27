using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class Comment : Base
    {
        public string Massage { get; set; }
        public string CommentBy { get; set; }

        public virtual ICollection<SubComment> SubComments { get; set; }

        public Comment()
        {
            SubComments = new HashSet<SubComment>();
        }
    }
}
