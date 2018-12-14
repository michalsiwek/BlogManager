namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentSubcategoryStringLenghtMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContentSubcategories", "Name", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.ContentSubcategories", "Description", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContentSubcategories", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.ContentSubcategories", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
