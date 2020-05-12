namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Genres : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "Genres", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mangas", "Genres");
        }
    }
}
