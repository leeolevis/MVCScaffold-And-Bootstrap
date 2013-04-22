using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApp4.Entities;

namespace WebApp4.Infrastructure
{
    public class WebApp4Context:DbContext
    {
        public WebApp4Context()
            : base("WebApp4")
        {
        }

        public DbSet<WebApp4.Entities.Role> Role { get; set; }

        public DbSet<WebApp4.Entities.User> User { get; set; }

        public DbSet<WebApp4.Entities.Permission> Permission { get; set; }

    }
}