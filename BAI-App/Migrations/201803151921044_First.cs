namespace BAI_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AllowedMessages",
                c => new
                    {
                        AllowID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        MessageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AllowID)
                .ForeignKey("dbo.Messages", t => t.MessageID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.MessageID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Mod = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        PasswordHash = c.String(),
                        Salt = c.String(),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AllowedMessages", "UserID", "dbo.Users");
            DropForeignKey("dbo.AllowedMessages", "MessageID", "dbo.Messages");
            DropIndex("dbo.AllowedMessages", new[] { "MessageID" });
            DropIndex("dbo.AllowedMessages", new[] { "UserID" });
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
            DropTable("dbo.AllowedMessages");
        }
    }
}
