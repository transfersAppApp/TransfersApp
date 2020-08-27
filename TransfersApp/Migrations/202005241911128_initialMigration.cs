namespace TransfersApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientPayments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TravelId = c.Guid(),
                        ClientId = c.Guid(),
                        PaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDone = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Travels", t => t.TravelId)
                .Index(t => t.TravelId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        InsuranceSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balancce = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinPassengers = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Travels",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TransferId = c.Guid(),
                        TravelStatusId = c.Int(),
                        DateTime = c.DateTime(),
                        DisplayName = c.String(),
                        From = c.String(),
                        Destination = c.String(),
                        StateCarNumber = c.String(),
                        ShuttleId = c.Guid(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TarrifId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        Tariff_Id = c.Int(),
                        Client_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tariffs", t => t.Tariff_Id)
                .ForeignKey("dbo.Transfers", t => t.TransferId)
                .ForeignKey("dbo.TravelStatus", t => t.TravelStatusId)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.TransferId)
                .Index(t => t.TravelStatusId)
                .Index(t => t.Tariff_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Confirmations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ClientId = c.Guid(),
                        Text = c.String(),
                        IsPositive = c.Boolean(nullable: false),
                        TravelId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Travels", t => t.TravelId)
                .Index(t => t.ClientId)
                .Index(t => t.TravelId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Text = c.String(),
                        TravelId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Travels", t => t.TravelId)
                .Index(t => t.TravelId);
            
            CreateTable(
                "dbo.Tariffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        From = c.String(),
                        Destination = c.String(),
                        Periodicity = c.Int(),
                        DepartureTime = c.DateTime(),
                        ArrivalTime = c.DateTime(),
                        StatusId = c.Int(),
                        MinimumClassId = c.Int(),
                        ShuttleId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        MinimalClass_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportClasses", t => t.MinimalClass_Id)
                .ForeignKey("dbo.Shuttles", t => t.ShuttleId)
                .ForeignKey("dbo.TransferStatus", t => t.StatusId)
                .Index(t => t.StatusId)
                .Index(t => t.ShuttleId)
                .Index(t => t.MinimalClass_Id);
            
            CreateTable(
                "dbo.TransportClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shuttles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        TimeSlotId = c.Int(),
                        StartRallyPointId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RallyPoints", t => t.StartRallyPointId)
                .ForeignKey("dbo.TimeSlots", t => t.TimeSlotId)
                .Index(t => t.TimeSlotId)
                .Index(t => t.StartRallyPointId);
            
            CreateTable(
                "dbo.RallyPoints",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Coordinates = c.String(),
                        Description = c.String(),
                        IsProxy = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Shuttle_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shuttles", t => t.Shuttle_Id)
                .Index(t => t.Shuttle_Id);
            
            CreateTable(
                "dbo.TimeSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hours = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransferStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Wishes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TravelId = c.Guid(),
                        WishText = c.String(),
                        ClientId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                        Transfer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Travels", t => t.TravelId)
                .ForeignKey("dbo.Transfers", t => t.Transfer_Id)
                .Index(t => t.TravelId)
                .Index(t => t.ClientId)
                .Index(t => t.Transfer_Id);
            
            CreateTable(
                "dbo.TravelStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientTransfers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TransferId = c.Guid(),
                        ClientId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Transfers", t => t.TransferId)
                .Index(t => t.TransferId)
                .Index(t => t.ClientId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShuttleModels", t => t.ShuttleModel_Id)
                .Index(t => t.ShuttleModel_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ShuttleModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        TimeSlotId = c.Int(),
                        StartRallyPointId = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RallyPointModels", t => t.StartRallyPointId)
                .ForeignKey("dbo.TimeSlotModels", t => t.TimeSlotId)
                .Index(t => t.TimeSlotId)
                .Index(t => t.StartRallyPointId);
            
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
                "dbo.TransferRallyPoints",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TransferId = c.Guid(),
                        RallyPointId = c.Guid(),
                        SortOrder = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RallyPoints", t => t.RallyPointId)
                .ForeignKey("dbo.Transfers", t => t.TransferId)
                .Index(t => t.TransferId)
                .Index(t => t.RallyPointId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TransferRallyPoints", "TransferId", "dbo.Transfers");
            DropForeignKey("dbo.TransferRallyPoints", "RallyPointId", "dbo.RallyPoints");
            DropForeignKey("dbo.ShuttleModels", "TimeSlotId", "dbo.TimeSlotModels");
            DropForeignKey("dbo.ShuttleModels", "StartRallyPointId", "dbo.RallyPointModels");
            DropForeignKey("dbo.RallyPointModels", "ShuttleModel_Id", "dbo.ShuttleModels");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ClientTransfers", "TransferId", "dbo.Transfers");
            DropForeignKey("dbo.ClientTransfers", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientPayments", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.ClientPayments", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Travels", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Travels", "TravelStatusId", "dbo.TravelStatus");
            DropForeignKey("dbo.Wishes", "Transfer_Id", "dbo.Transfers");
            DropForeignKey("dbo.Wishes", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Wishes", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Travels", "TransferId", "dbo.Transfers");
            DropForeignKey("dbo.Transfers", "StatusId", "dbo.TransferStatus");
            DropForeignKey("dbo.Transfers", "ShuttleId", "dbo.Shuttles");
            DropForeignKey("dbo.Shuttles", "TimeSlotId", "dbo.TimeSlots");
            DropForeignKey("dbo.Shuttles", "StartRallyPointId", "dbo.RallyPoints");
            DropForeignKey("dbo.RallyPoints", "Shuttle_Id", "dbo.Shuttles");
            DropForeignKey("dbo.Transfers", "MinimalClass_Id", "dbo.TransportClasses");
            DropForeignKey("dbo.Travels", "Tariff_Id", "dbo.Tariffs");
            DropForeignKey("dbo.Messages", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Confirmations", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Confirmations", "ClientId", "dbo.Clients");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.TransferRallyPoints", new[] { "RallyPointId" });
            DropIndex("dbo.TransferRallyPoints", new[] { "TransferId" });
            DropIndex("dbo.ShuttleModels", new[] { "StartRallyPointId" });
            DropIndex("dbo.ShuttleModels", new[] { "TimeSlotId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RallyPointModels", new[] { "ShuttleModel_Id" });
            DropIndex("dbo.ClientTransfers", new[] { "ClientId" });
            DropIndex("dbo.ClientTransfers", new[] { "TransferId" });
            DropIndex("dbo.Wishes", new[] { "Transfer_Id" });
            DropIndex("dbo.Wishes", new[] { "ClientId" });
            DropIndex("dbo.Wishes", new[] { "TravelId" });
            DropIndex("dbo.RallyPoints", new[] { "Shuttle_Id" });
            DropIndex("dbo.Shuttles", new[] { "StartRallyPointId" });
            DropIndex("dbo.Shuttles", new[] { "TimeSlotId" });
            DropIndex("dbo.Transfers", new[] { "MinimalClass_Id" });
            DropIndex("dbo.Transfers", new[] { "ShuttleId" });
            DropIndex("dbo.Transfers", new[] { "StatusId" });
            DropIndex("dbo.Messages", new[] { "TravelId" });
            DropIndex("dbo.Confirmations", new[] { "TravelId" });
            DropIndex("dbo.Confirmations", new[] { "ClientId" });
            DropIndex("dbo.Travels", new[] { "Client_Id" });
            DropIndex("dbo.Travels", new[] { "Tariff_Id" });
            DropIndex("dbo.Travels", new[] { "TravelStatusId" });
            DropIndex("dbo.Travels", new[] { "TransferId" });
            DropIndex("dbo.ClientPayments", new[] { "ClientId" });
            DropIndex("dbo.ClientPayments", new[] { "TravelId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TransferRallyPoints");
            DropTable("dbo.TimeSlotModels");
            DropTable("dbo.ShuttleModels");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RallyPointModels");
            DropTable("dbo.MessageModels");
            DropTable("dbo.ClientTransfers");
            DropTable("dbo.TravelStatus");
            DropTable("dbo.Wishes");
            DropTable("dbo.TransferStatus");
            DropTable("dbo.TimeSlots");
            DropTable("dbo.RallyPoints");
            DropTable("dbo.Shuttles");
            DropTable("dbo.TransportClasses");
            DropTable("dbo.Transfers");
            DropTable("dbo.Tariffs");
            DropTable("dbo.Messages");
            DropTable("dbo.Confirmations");
            DropTable("dbo.Travels");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientPayments");
        }
    }
}
