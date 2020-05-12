namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEnumReadingState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mangas", "Read", c => c.Int(nullable: false));
            DropColumn("dbo.Mangas", "ReadingState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mangas", "ReadingState", c => c.String());
            DropColumn("dbo.Mangas", "Read");
        }
    }
}
