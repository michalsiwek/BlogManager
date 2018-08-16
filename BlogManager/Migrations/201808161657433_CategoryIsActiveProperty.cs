namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryIsActiveProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntryCategories", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntryCategories", "IsActive");
        }
    }
}
