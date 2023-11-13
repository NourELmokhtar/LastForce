using Forces.Application.Enums;
namespace Forces.Application.Responses.Identity
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureDataUrl { get; set; }
        public int? Rank { get; set; }
        public string JobTitle { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public UserType? UserType { get; set; }
        public DepartType? DepartType { get; set; }
        public int? DepartId { get; set; }
    }
}