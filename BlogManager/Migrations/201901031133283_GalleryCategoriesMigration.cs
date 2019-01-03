namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GalleryCategoriesMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galleries", "ContentCategory_Id", c => c.Int());
            AddColumn("dbo.Galleries", "ContentSubcategory_Id", c => c.Int());
            CreateIndex("dbo.Galleries", "ContentCategory_Id");
            CreateIndex("dbo.Galleries", "ContentSubcategory_Id");
            AddForeignKey("dbo.Galleries", "ContentCategory_Id", "dbo.ContentCategories", "Id");
            AddForeignKey("dbo.Galleries", "ContentSubcategory_Id", "dbo.ContentSubcategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Galleries", "ContentSubcategory_Id", "dbo.ContentSubcategories");
            DropForeignKey("dbo.Galleries", "ContentCategory_Id", "dbo.ContentCategories");
            DropIndex("dbo.Galleries", new[] { "ContentSubcategory_Id" });
            DropIndex("dbo.Galleries", new[] { "ContentCategory_Id" });
            DropColumn("dbo.Galleries", "ContentSubcategory_Id");
            DropColumn("dbo.Galleries", "ContentCategory_Id");
        }
    }
}
