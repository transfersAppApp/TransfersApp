namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paidTravel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Travels", "Paid", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Travels", "Paid");
        }
    }
}
