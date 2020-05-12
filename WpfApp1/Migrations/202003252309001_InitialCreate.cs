namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mangas",
                c => new
                    {
                        MangaId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        SourceImage = c.String(),
                        YearReleased = c.Int(nullable: false),
                        Volumes = c.Int(nullable: false),
                        Synopsis = c.String(),
                        FaveButton = c.String(),
                    })
                .PrimaryKey(t => t.MangaId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Text = c.String(),
                        MangaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Mangas", t => t.MangaId, cascadeDelete: true)
                .Index(t => t.MangaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "MangaId", "dbo.Mangas");
            DropIndex("dbo.Reviews", new[] { "MangaId" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Mangas");
        }
    }
}
