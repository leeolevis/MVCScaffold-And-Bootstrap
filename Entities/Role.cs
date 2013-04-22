using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp4.Entities
{
    public class Role : Entity
    {
        //Membership required
        //[Key()]
        //public virtual Guid RoleId { get; set; }

        [DisplayFormat]
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Permission> Roles { get; set; }

        [Required()]
        [MaxLength(100)]
        [Display(Name="½ÇÉ«Ãû³Æ")]
        public virtual string RoleName { get; set; }

        //Optional
        [MaxLength(250)]
        [Display(Name = "½ÇÉ«ÃèÊö")]
        public virtual string Description { get; set; }
    }
}