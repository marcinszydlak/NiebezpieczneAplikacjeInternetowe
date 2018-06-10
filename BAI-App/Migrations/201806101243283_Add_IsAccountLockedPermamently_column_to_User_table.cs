namespace Bai_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsAccountLockedPermamently_column_to_User_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsAccountLockedPermamently", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsAccountLockedPermamently");
        }
    }
}
