namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minPass2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "MinPassengers", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "MinPassengers", c => c.Int(nullable: false));
        }
    }
}
