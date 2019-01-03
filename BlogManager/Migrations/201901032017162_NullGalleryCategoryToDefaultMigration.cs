namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullGalleryCategoryToDefaultMigration : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Galleries SET ContentCategory_Id = 1 WHERE ContentCategory_Id IS NULL");
        }
        
        public override void Down()
        {
        }
    }
}
