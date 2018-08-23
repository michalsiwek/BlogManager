namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentParagraphs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Paragraphs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntryId = c.Int(nullable: false),
                        SubContentId = c.Int(nullable: false),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entries", t => t.EntryId, cascadeDelete: true)
                .Index(t => t.EntryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Paragraphs", "EntryId", "dbo.Entries");
            DropIndex("dbo.Paragraphs", new[] { "EntryId" });
            DropTable("dbo.Paragraphs");
        }
    }
}
