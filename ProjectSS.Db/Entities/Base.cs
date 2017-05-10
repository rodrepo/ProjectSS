using ProjectSS.Db.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public abstract class Base : IAuditable
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public bool IsActive { get; set; }

        #region IAuditable
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public DateTime? CreatedDateConverted
        {
            get
            {
                return ConvertUTC(CreatedDate);
            }
        }
        public DateTime? ModifiedDateConverted
        {
            get
            {
                return ConvertUTC(ModifiedDate);
            }
        }

        private DateTime? ConvertUTC(DateTime? date)
        {
            if (date != null)
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                date = TimeZoneInfo.ConvertTimeFromUtc(date.Value, timeZone);
            }
            return date;
        }
        #endregion
    }
}
