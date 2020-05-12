namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompareMethod : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Mangas", "Genres");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mangas", "Genres", c => c.String());
        }
    }
}
