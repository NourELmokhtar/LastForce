using Forces.Application.Enums;
using Forces.Application.Interfaces.Chat;
using Forces.Application.Models;
using Forces.Application.Models.Chat;
using Forces.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Infrastructure.Models.Identity
{
    public class Appuser : IdentityUser<string>, IChatUser, IAuditableEntity<string>, IScopedEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string CreatedBy { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePictureDataUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public int? ForceID { get; set; }
        public int? BaseID { get; set; }
        public int? BaseSectionID { get; set; }
        [ForeignKey("ForceID")]
        public virtual Application.Models.Forces Force { get; set; }
        [ForeignKey("BaseID")]
        public virtual Application.Models.Bases Base { get; set; }


        public int? Rank { get; set; }
        public string JobTitle { get; set; }
        public string InternalId { get; set; }
        public UserType? UserType { get; set; }
        public DepartType? DepartmentType { get; set; }
        public int? DepoDepartmentID { get; set; }
        public int? HQDepartmentID { get; set; }
        [ForeignKey("DepoDepartmentID")]
        public virtual DepoDepartment DepoDepartment { get; set; }
        [ForeignKey("HQDepartmentID")]
        public virtual HQDepartment HQDepartment { get; set; }
        public int? DefaultVoteCodeID { get; set; }
        [ForeignKey("BaseSectionID")]
        public virtual Application.Models.BasesSections BaseSection { get; set; }
        public virtual ICollection<ChatHistory<Appuser>> ChatHistoryFromUsers { get; set; }
        public virtual ICollection<ChatHistory<Appuser>> ChatHistoryToUsers { get; set; }

        public virtual ICollection<VoteCodes> HoldingVoteCodes { get; set; }
        public virtual ICollection<NotificationsUsers> Notifications { get; set; }


        public Appuser()
        {
            ChatHistoryFromUsers = new HashSet<ChatHistory<Appuser>>();
            ChatHistoryToUsers = new HashSet<ChatHistory<Appuser>>();
            HoldingVoteCodes = new HashSet<VoteCodes>();
            Notifications = new HashSet<NotificationsUsers>();
        }
    }
}