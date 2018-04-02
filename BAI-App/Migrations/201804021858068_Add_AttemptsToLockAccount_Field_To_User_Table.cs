namespace Bai_APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AttemptsToLockAccount_Field_To_User_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AttemptsToLockAccount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AttemptsToLockAccount");
        }
    }
}
