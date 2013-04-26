using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApp4.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        [StringLength(50)]
        public string CreateDate { get; set; }
        [StringLength(50)]
        public string Origin { get; set; }
        [StringLength(20)]
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}