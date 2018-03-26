namespace Bai_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageViewModels",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        MessageText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MessageViewModels");
        }
    }
}
