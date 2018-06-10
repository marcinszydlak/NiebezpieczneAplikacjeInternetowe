namespace Bai_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_columns_to_AnonymousUser_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnonymousUsers", "AccountLockedTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.AnonymousUsers", "FailedLoginAttempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnonymousUsers", "FailedLoginAttempts");
            DropColumn("dbo.AnonymousUsers", "AccountLockedTo");
        }
    }
}
