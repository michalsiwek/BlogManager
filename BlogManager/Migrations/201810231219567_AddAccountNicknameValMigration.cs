namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountNicknameValMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nickname", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 100));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 255));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 255));
            DropColumn("dbo.AspNetUsers", "Nickname");
        }
    }
}
