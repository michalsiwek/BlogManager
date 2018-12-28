namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentSubcategoriesRenameMigration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Entries", name: "Subcategory_Id", newName: "ContentSubcategory_Id");
            RenameIndex(table: "dbo.Entries", name: "IX_Subcategory_Id", newName: "IX_ContentSubcategory_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Entries", name: "IX_ContentSubcategory_Id", newName: "IX_Subcategory_Id");
            RenameColumn(table: "dbo.Entries", name: "ContentSubcategory_Id", newName: "Subcategory_Id");
        }
    }
}
