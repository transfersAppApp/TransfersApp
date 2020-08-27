namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "LengthFromLastRallyPoint", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transfers", "Length", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Shuttles", "Price", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ShuttleModels", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShuttleModels", "Price");
            DropColumn("dbo.Shuttles", "Price");
            DropColumn("dbo.Transfers", "Length");
            DropColumn("dbo.Transfers", "LengthFromLastRallyPoint");
        }
    }
}
