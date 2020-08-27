namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "HomeAddressLocaction", c => c.String());
            AddColumn("dbo.Clients", "WorkAdressLocaction", c => c.String());
            AddColumn("dbo.Clients", "HomeAddress", c => c.String());
            AddColumn("dbo.Clients", "WorkAdress", c => c.String());
            AddColumn("dbo.Clients", "WorkArrivingTime", c => c.String());
            AddColumn("dbo.Clients", "WorkDepartureTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "WorkDepartureTime");
            DropColumn("dbo.Clients", "WorkArrivingTime");
            DropColumn("dbo.Clients", "WorkAdress");
            DropColumn("dbo.Clients", "HomeAddress");
            DropColumn("dbo.Clients", "WorkAdressLocaction");
            DropColumn("dbo.Clients", "HomeAddressLocaction");
        }
    }
}
