using Forces.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Forces.Infrastructure.Models.Identity
{
    public class AppRole : IdentityRole, IAuditableEntity<string>, IScopedEntity
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; }
        public int? ForceID { get; set; }
        public int? BaseID { get; set; }
        public int? BaseSectionID { get; set; }

        public AppRole() : base()
        {

            RoleClaims = new HashSet<AppRoleClaim>();
        }


        public AppRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<AppRoleClaim>();
            Description = roleDescription;
        }
    }
}