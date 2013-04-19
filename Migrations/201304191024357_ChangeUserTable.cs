namespace WebApp4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "CreateDate");
            DropColumn("dbo.Users", "FirstName");
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "TimeZone");
            DropColumn("dbo.Users", "Culture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Culture", c => c.String());
            AddColumn("dbo.Users", "TimeZone", c => c.String());
            AddColumn("dbo.Users", "LastName", c => c.String());
            AddColumn("dbo.Users", "FirstName", c => c.String());
            AddColumn("dbo.Users", "CreateDate", c => c.DateTime());
        }
    }
}
