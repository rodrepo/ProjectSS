using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class SubComment : Base
    {
        public string Massage { get; set; }
        public string CommentBy { get; set; }

        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
