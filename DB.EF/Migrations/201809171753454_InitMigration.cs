namespace DB.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        PatronymicName = c.String(),
                        Position = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisitLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PacientId = c.Int(),
                        DoctorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacients", t => t.PacientId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .Index(t => t.PacientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Pacients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(nullable: false),
                        PatronymicName = c.String(),
                        PacientType = c.Int(nullable: false),
                        Sity = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        BuildingNumber = c.String(),
                        FlatNumber = c.String(),
                        PacientPhoneNumber = c.String(),
                        ParentFirstName = c.String(),
                        ParentLastName = c.String(),
                        ParentPatronymicName = c.String(),
                        ParentPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Document = c.Binary(),
                        PacientId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacients", t => t.PacientId)
                .Index(t => t.PacientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitLogs", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.VisitLogs", "PacientId", "dbo.Pacients");
            DropForeignKey("dbo.Documents", "PacientId", "dbo.Pacients");
            DropIndex("dbo.Documents", new[] { "PacientId" });
            DropIndex("dbo.VisitLogs", new[] { "DoctorId" });
            DropIndex("dbo.VisitLogs", new[] { "PacientId" });
            DropTable("dbo.Documents");
            DropTable("dbo.Pacients");
            DropTable("dbo.VisitLogs");
            DropTable("dbo.Doctors");
        }
    }
}
