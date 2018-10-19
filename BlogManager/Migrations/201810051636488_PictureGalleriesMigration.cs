namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PictureGalleriesMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        Descripton = c.String(),
                        Author = c.String(),
                        Url = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        GalleryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Galleries", t => t.GalleryId, cascadeDelete: true)
                .Index(t => t.GalleryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "GalleryId", "dbo.Galleries");
            DropForeignKey("dbo.Galleries", "Account_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Pictures", new[] { "GalleryId" });
            DropIndex("dbo.Galleries", new[] { "Account_Id" });
            DropTable("dbo.Pictures");
            DropTable("dbo.Galleries");
        }
    }
}
