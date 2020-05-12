namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YearReleasedToDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "DateReleased", c => c.DateTime(nullable: false));
            DropColumn("dbo.Mangas", "YearReleased");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mangas", "YearReleased", c => c.Int(nullable: false));
            DropColumn("dbo.Mangas", "DateReleased");
        }
    }
}
