using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp4.Entities
{
    public class PermissionsInRoles : Entity
    {
        //[Key]
        //public int Id { get; set; }

        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        //[InverseProperty("PermissionsInRoles")]
        public Role Role { get; set; }

        //[InverseProperty("PermissionsInRoles")]
        public Permission Permission { get; set; }
    }
}