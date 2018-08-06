namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsVerifiedToIsActiveRename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "IsVerified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsVerified", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "IsActive");
        }
    }
}
