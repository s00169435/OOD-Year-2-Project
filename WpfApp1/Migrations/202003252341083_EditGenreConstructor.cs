namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditGenreConstructor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "Genre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mangas", "Genre");
        }
    }
}
