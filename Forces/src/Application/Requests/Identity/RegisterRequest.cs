using Forces.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Forces.Application.Requests.Identity
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        public bool ActivateUser { get; set; } = true;
        public bool AutoConfirmEmail { get; set; } = true;
        public int? ForceID { get; set; }
        public int? BaseID { get; set; }
        public int? BaseSectionID { get; set; }
        public int? Rank { get; set; } = 0;
        public string JobTitle { get; set; }
        public string InternalId { get; set; }
        public UserType UserType { get; set; }
        public DepartType? DepartmentType { get; set; }
        public int? DepoDepartmentID { get; set; }
        public int? HQDepartmentID { get; set; }
        public int? DefaultVoteCodeID { get; set; }

    }

    public class EditUserRequest
    {
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public bool ActivateUser { get; set; } = true;
        public bool AutoConfirmEmail { get; set; } = true;
        public int? ForceID { get; set; }
        public int? BaseID { get; set; }
        public int? BaseSectionID { get; set; }
        public int? Rank { get; set; } = 0;
        public string JobTitle { get; set; }
        public string InternalId { get; set; }
        public UserType UserType { get; set; }
        public DepartType? DepartmentType { get; set; }
        public int? DepoDepartmentID { get; set; }
        public int? HQDepartmentID { get; set; }
        public int? DefaultVoteCodeID { get; set; }

    }
}