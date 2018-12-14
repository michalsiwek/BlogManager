namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntrySubcategoriesPropertyMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSubcategories", "Entry_Id", c => c.Int());
            CreateIndex("dbo.ContentSubcategories", "Entry_Id");
            AddForeignKey("dbo.ContentSubcategories", "Entry_Id", "dbo.Entries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentSubcategories", "Entry_Id", "dbo.Entries");
            DropIndex("dbo.ContentSubcategories", new[] { "Entry_Id" });
            DropColumn("dbo.ContentSubcategories", "Entry_Id");
        }
    }
}
