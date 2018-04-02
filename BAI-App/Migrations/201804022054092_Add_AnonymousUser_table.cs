namespace Bai_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AnonymousUser_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnonymousUsers",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Login);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AnonymousUsers");
        }
    }
}
