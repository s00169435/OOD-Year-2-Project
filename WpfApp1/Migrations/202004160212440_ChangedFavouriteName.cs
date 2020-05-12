namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFavouriteName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "Favourite", c => c.Boolean(nullable: false));
            DropColumn("dbo.Mangas", "AddedToFave");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mangas", "AddedToFave", c => c.Boolean(nullable: false));
            DropColumn("dbo.Mangas", "Favourite");
        }
    }
}
