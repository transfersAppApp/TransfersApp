namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clean : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RallyPointModels", "ShuttleModel_Id", "dbo.ShuttleModels");
            DropForeignKey("dbo.ShuttleModels", "StartRallyPointId", "dbo.RallyPointModels");
            DropForeignKey("dbo.ShuttleModels", "TimeSlotId", "dbo.TimeSlotModels");
            DropIndex("dbo.RallyPointModels", new[] { "ShuttleModel_Id" });
            DropIndex("dbo.ShuttleModels", new[] { "TimeSlotId" });
            DropIndex("dbo.ShuttleModels", new[] { "StartRallyPointId" });
            DropTable("dbo.MessageModels");
            DropTable("dbo.RallyPointModels");
            DropTable("dbo.ShuttleModels");
            DropTable("dbo.TimeSlotModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TimeSlotModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hours = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShuttleModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        TimeSlotId = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        StartRallyPointId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RallyPointModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Coordinates = c.String(),
                        Description = c.String(),
                        IsProxy = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ShuttleModel_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                        TravelId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ShuttleModels", "StartRallyPointId");
            CreateIndex("dbo.ShuttleModels", "TimeSlotId");
            CreateIndex("dbo.RallyPointModels", "ShuttleModel_Id");
            AddForeignKey("dbo.ShuttleModels", "TimeSlotId", "dbo.TimeSlotModels", "Id");
            AddForeignKey("dbo.ShuttleModels", "StartRallyPointId", "dbo.RallyPointModels", "Id");
            AddForeignKey("dbo.RallyPointModels", "ShuttleModel_Id", "dbo.ShuttleModels", "Id");
        }
    }
}
