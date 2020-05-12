namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReadingState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "ReadingState", c => c.Int(nullable: false));
            DropColumn("dbo.Mangas", "Read");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mangas", "Read", c => c.Int(nullable: false));
            DropColumn("dbo.Mangas", "ReadingState");
        }
    }
}
