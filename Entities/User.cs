using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp4.Entities
{
    public class User : Entity
    {

        //Membership required
        //[Key()]
        //public virtual Guid UserId { get; set; }
        [Required()]
        [MaxLength(20)]
        [Display(Name = "用户名称")]
        public virtual string Username { get; set; }
        [Required()]
        [MaxLength(250)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮箱")]
        public virtual string Email { get; set; }
        [Required()]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public virtual string Password { get; set; }
        [Display(Name = "是否确认")]
        public virtual bool IsConfirmed { get; set; }
        [ScaffoldColumn(false)]
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }
        [Display(Name = "密码失效日期")]
        public virtual Nullable<DateTime> LastPasswordFailureDate { get; set; }
        [Display(Name = "确认令牌")]
        public virtual string ConfirmationToken { get; set; }
        //[Display(Name = "创建日期")]
        //public virtual Nullable<DateTime> CreateDate { get; set; }
        [Display(Name = "密码修改日期")]
        public virtual Nullable<DateTime> PasswordChangedDate { get; set; }
        [Display(Name = "密码验证令牌")]
        public virtual string PasswordVerificationToken { get; set; }
        [Display(Name = "令牌过期时间")]
        public virtual Nullable<DateTime> PasswordVerificationTokenExpirationDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        //Optional
        //public virtual string FirstName { get; set; }
        //public virtual string LastName { get; set; }
        //public virtual string TimeZone { get; set; }
        //public virtual string Culture { get; set; }

    }
}