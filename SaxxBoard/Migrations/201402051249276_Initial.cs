namespace SaxxBoard.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataPoints",
                c => new
                    {
                        Id = c.Int(false, true),
                        WidgetIdentifier = c.String(),
                        SeriesIndex = c.Int(false),
                        DateTime = c.DateTime(false),
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
