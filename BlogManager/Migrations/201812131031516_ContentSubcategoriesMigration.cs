namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentSubcategoriesMigration : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Entries", name: "EntryCategory_Id", newName: "ContentCategory_Id");
            //RenameIndex(table: "dbo.Entries", name: "IX_EntryCategory_Id", newName: "IX_ContentCategory_Id");
            CreateTable(
                "dbo.ContentSubcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 255),
                        CreateDate = c.DateTime(nullable: false),
                        LastModification = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        ContentCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContentCategories", t => t.ContentCategory_Id)
                .Index(t => t.ContentCategory_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentSubcategories", "ContentCategory_Id", "dbo.ContentCategories");
            DropIndex("dbo.ContentSubcategories", new[] { "ContentCategory_Id" });
            DropTable("dbo.ContentSubcategories");
            RenameIndex(table: "dbo.Entries", name: "IX_ContentCategory_Id", newName: "IX_EntryCategory_Id");
            RenameColumn(table: "dbo.Entries", name: "ContentCategory_Id", newName: "EntryCategory_Id");
        }
    }
}
