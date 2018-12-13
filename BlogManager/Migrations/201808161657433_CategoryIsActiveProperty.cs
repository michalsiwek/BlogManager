namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryIsActiveProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentCategories", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentCategories", "IsActive");
        }
    }
}
