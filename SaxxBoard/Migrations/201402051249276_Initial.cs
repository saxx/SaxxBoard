namespace SaxxBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WidgetIdentifier = c.String(),
                        SeriesIndex = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Value = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataPoints");
        }
    }
}
