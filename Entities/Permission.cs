using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp4.Entities
{
    public class Permission : Entity
    {
        //public Permission()
        //{
        //    PermissionsInRoles = new List<PermissionsInRoles>();
        //}

        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //[Display(Name = "权限ID")]
        //public int PermissionId { get; set; }

        [Display(Name = "权限名称")]
        [MaxLength(100)]
        public virtual string PermissionName { get; set; }


        //Optional
        [MaxLength(250)]
        [Display(Name = "权限描述")]
        public virtual string Description { get; set; }

        [Display(Name = "权限父项")]
        public virtual Guid ParentId { get; set; }

        //public Enum.PermissionTypeEnum PermissionType { get; set; }
        public Enum.PermissionTypeEnum PermissionTypeEnum { get; set; }

        [Display(Name = "权限类型")]
        public virtual int PermissionType
        {
            get { return (int)PermissionTypeEnum; }
            set { PermissionTypeEnum = (Enum.PermissionTypeEnum)value; }
        }

        public virtual ICollection<Role> Roles { get; set; }
        //[ForeignKey("PermissionId")]
        //public ICollection<PermissionsInRoles> PermissionsInRoles { set; get; }
    }
}