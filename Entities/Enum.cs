using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace WebApp4.Entities
{
    public class Enum
    {
        public enum PermissionTypeEnum
        {
            [Description("页面类")]
            页面类,
            [Description("操作类")]
            操作类
        }
    }
}