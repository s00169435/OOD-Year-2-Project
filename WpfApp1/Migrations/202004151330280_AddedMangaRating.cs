namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMangaRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "Rating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mangas", "Rating");
        }
    }
}
