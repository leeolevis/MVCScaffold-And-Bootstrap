using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApp4.Infrastructure;

namespace WebApp4.Membership
{
    public class CodeFirstContextInit : DropCreateDatabaseAlways<WebApp4Context>
    {

        protected override void Seed(WebApp4Context context)
        {

            CodeFirstSecurity.CreateAccount("Demo", "Demo", "demo@demo.com");

        }

    }
}