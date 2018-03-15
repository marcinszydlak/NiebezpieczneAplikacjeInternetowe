namespace BAI_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserLogin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserLogin");
        }
    }
}
