namespace Bai_APP.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddFieldsToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastSuccessLoginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "LastFailLoginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "FailedLoginAttemptsCountSinceLastSuccessful", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "LastLogin");
        }

        public override void Down()
        {
            AddColumn("dbo.Users", "LastLogin", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "FailedLoginAttemptsCountSinceLastSuccessful");
            DropColumn("dbo.Users", "LastFailLoginDate");
            DropColumn("dbo.Users", "LastSuccessLoginDate");
        }
    }
}
