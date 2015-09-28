namespace Iris.Datalayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_LastActivityProperty_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastActivity", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastActivity");
        }
    }
}
