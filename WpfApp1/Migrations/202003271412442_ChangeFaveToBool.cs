namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFaveToBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "AddedToFave", c => c.Boolean(nullable: false));
            DropColumn("dbo.Mangas", "FaveButton");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mangas", "FaveButton", c => c.String());
            DropColumn("dbo.Mangas", "AddedToFave");
        }
    }
}
