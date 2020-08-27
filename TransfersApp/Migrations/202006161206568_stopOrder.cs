namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stopOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transfers", "ShuttleStopOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "ShuttleStopOrder");
        }
    }
}
