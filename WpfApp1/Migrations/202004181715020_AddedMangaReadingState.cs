namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMangaReadingState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "ReadingState", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mangas", "ReadingState");
        }
    }
}
