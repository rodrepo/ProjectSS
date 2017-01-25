using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectSS.Common;

namespace ProjectSS.Web.Models.admin
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid characters")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid characters")]
        public string MiddleName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid characters")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<RoleViewModel> Roles { get; set; }
        public string RoleId { get; set; }

        public int AccountId { get; set; }
        public string AccountName { get; set; }

        public string Displayname
        {
            get
            {
                return (FirstName +" "+ MiddleName +" "+ LastName );
            }
        }
    }

    public class ActivateViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = " ")]
        public DateTime Birthday { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

    }

}