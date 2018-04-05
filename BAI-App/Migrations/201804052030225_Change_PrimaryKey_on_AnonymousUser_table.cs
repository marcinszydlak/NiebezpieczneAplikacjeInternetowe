namespace Bai_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_PrimaryKey_on_AnonymousUser_table : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AnonymousUsers");
            AddColumn("dbo.AnonymousUsers", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AnonymousUsers", "Login", c => c.String());
            AddPrimaryKey("dbo.AnonymousUsers", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AnonymousUsers");
            AlterColumn("dbo.AnonymousUsers", "Login", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AnonymousUsers", "Id");
            AddPrimaryKey("dbo.AnonymousUsers", "Login");
        }
    }
}
