namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GalleriesMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pictures", "Gallery_Id", "dbo.Galleries");
            DropIndex("dbo.Pictures", new[] { "Gallery_Id" });
            RenameColumn(table: "dbo.Pictures", name: "Gallery_Id", newName: "GalleryId");
            AlterColumn("dbo.Pictures", "GalleryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pictures", "GalleryId");
            AddForeignKey("dbo.Pictures", "GalleryId", "dbo.Galleries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "GalleryId", "dbo.Galleries");
            DropIndex("dbo.Pictures", new[] { "GalleryId" });
            AlterColumn("dbo.Pictures", "GalleryId", c => c.Int());
            RenameColumn(table: "dbo.Pictures", name: "GalleryId", newName: "Gallery_Id");
            CreateIndex("dbo.Pictures", "Gallery_Id");
            AddForeignKey("dbo.Pictures", "Gallery_Id", "dbo.Galleries", "Id");
        }
    }
}
