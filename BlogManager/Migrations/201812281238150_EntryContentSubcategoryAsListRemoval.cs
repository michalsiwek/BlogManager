namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryContentSubcategoryAsListRemoval : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContentSubcategories", "Entry_Id", "dbo.Entries");
            DropIndex("dbo.ContentSubcategories", new[] { "Entry_Id" });
            AddColumn("dbo.Entries", "Subcategory_Id", c => c.Int());
            CreateIndex("dbo.Entries", "Subcategory_Id");
            AddForeignKey("dbo.Entries", "Subcategory_Id", "dbo.ContentSubcategories", "Id");
            DropColumn("dbo.ContentSubcategories", "Entry_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContentSubcategories", "Entry_Id", c => c.Int());
            DropForeignKey("dbo.Entries", "Subcategory_Id", "dbo.ContentSubcategories");
            DropIndex("dbo.Entries", new[] { "Subcategory_Id" });
            DropColumn("dbo.Entries", "Subcategory_Id");
            CreateIndex("dbo.ContentSubcategories", "Entry_Id");
            AddForeignKey("dbo.ContentSubcategories", "Entry_Id", "dbo.Entries", "Id");
        }
    }
}
