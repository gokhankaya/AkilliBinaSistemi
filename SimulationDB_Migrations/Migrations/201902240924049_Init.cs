namespace SimulationDB_Migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Actor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.Habits",
                c => new
                    {
                        HabitID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.HabitID);
            
            CreateTable(
                "public.OperationHabitMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaxDuration = c.Int(nullable: false),
                        MinDuration = c.Int(nullable: false),
                        OperationID = c.Int(),
                        HabitID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Habits", t => t.HabitID)
                .ForeignKey("public.Operations", t => t.OperationID)
                .Index(t => t.OperationID)
                .Index(t => t.HabitID);
            
            CreateTable(
                "public.Operations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 6),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.OperationDevices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Sira = c.Int(nullable: false),
                        ActionName = c.String(),
                        OperationID = c.Int(nullable: false),
                        DeviceBaseID = c.Int(nullable: false),
                        AreaID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.DeviceBases", t => t.DeviceBaseID, cascadeDelete: true)
                .ForeignKey("public.AreaBases", t => t.AreaID)
                .ForeignKey("public.Operations", t => t.OperationID, cascadeDelete: true)
                .Index(t => t.OperationID)
                .Index(t => t.DeviceBaseID)
                .Index(t => t.AreaID);
            
            CreateTable(
                "public.AreaBases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.DeviceBases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ip = c.String(),
                        AreaID = c.Int(nullable: false),
                        state = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.AreaBases", t => t.AreaID, cascadeDelete: true)
                .Index(t => t.AreaID);
            
            CreateTable(
                "public.GraphNodeDeviceMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NodeName = c.String(),
                        DeviceID = c.Int(nullable: false),
                        GraphID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.DeviceBases", t => t.DeviceID, cascadeDelete: true)
                .ForeignKey("public.GraphObjects", t => t.GraphID, cascadeDelete: true)
                .Index(t => t.DeviceID)
                .Index(t => t.GraphID);
            
            CreateTable(
                "public.GraphObjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MatrixValue = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.Scenarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.HabitActors",
                c => new
                    {
                        Habit_HabitID = c.Int(nullable: false),
                        Actor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Habit_HabitID, t.Actor_ID })
                .ForeignKey("public.Habits", t => t.Habit_HabitID, cascadeDelete: true)
                .ForeignKey("public.Actor", t => t.Actor_ID, cascadeDelete: true)
                .Index(t => t.Habit_HabitID)
                .Index(t => t.Actor_ID);
            
            CreateTable(
                "public.ScenarioActors",
                c => new
                    {
                        Scenario_ID = c.Int(nullable: false),
                        Actor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scenario_ID, t.Actor_ID })
                .ForeignKey("public.Scenarios", t => t.Scenario_ID, cascadeDelete: true)
                .ForeignKey("public.Actor", t => t.Actor_ID, cascadeDelete: true)
                .Index(t => t.Scenario_ID)
                .Index(t => t.Actor_ID);
            
            CreateTable(
                "public.ScenarioAreaBases",
                c => new
                    {
                        Scenario_ID = c.Int(nullable: false),
                        AreaBase_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scenario_ID, t.AreaBase_ID })
                .ForeignKey("public.Scenarios", t => t.Scenario_ID, cascadeDelete: true)
                .ForeignKey("public.AreaBases", t => t.AreaBase_ID, cascadeDelete: true)
                .Index(t => t.Scenario_ID)
                .Index(t => t.AreaBase_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.OperationHabitMappings", "OperationID", "public.Operations");
            DropForeignKey("public.OperationDevices", "OperationID", "public.Operations");
            DropForeignKey("public.ScenarioAreaBases", "AreaBase_ID", "public.AreaBases");
            DropForeignKey("public.ScenarioAreaBases", "Scenario_ID", "public.Scenarios");
            DropForeignKey("public.ScenarioActors", "Actor_ID", "public.Actor");
            DropForeignKey("public.ScenarioActors", "Scenario_ID", "public.Scenarios");
            DropForeignKey("public.OperationDevices", "AreaID", "public.AreaBases");
            DropForeignKey("public.GraphNodeDeviceMappings", "GraphID", "public.GraphObjects");
            DropForeignKey("public.GraphNodeDeviceMappings", "DeviceID", "public.DeviceBases");
            DropForeignKey("public.OperationDevices", "DeviceBaseID", "public.DeviceBases");
            DropForeignKey("public.DeviceBases", "AreaID", "public.AreaBases");
            DropForeignKey("public.OperationHabitMappings", "HabitID", "public.Habits");
            DropForeignKey("public.HabitActors", "Actor_ID", "public.Actor");
            DropForeignKey("public.HabitActors", "Habit_HabitID", "public.Habits");
            DropIndex("public.ScenarioAreaBases", new[] { "AreaBase_ID" });
            DropIndex("public.ScenarioAreaBases", new[] { "Scenario_ID" });
            DropIndex("public.ScenarioActors", new[] { "Actor_ID" });
            DropIndex("public.ScenarioActors", new[] { "Scenario_ID" });
            DropIndex("public.HabitActors", new[] { "Actor_ID" });
            DropIndex("public.HabitActors", new[] { "Habit_HabitID" });
            DropIndex("public.GraphNodeDeviceMappings", new[] { "GraphID" });
            DropIndex("public.GraphNodeDeviceMappings", new[] { "DeviceID" });
            DropIndex("public.DeviceBases", new[] { "AreaID" });
            DropIndex("public.OperationDevices", new[] { "AreaID" });
            DropIndex("public.OperationDevices", new[] { "DeviceBaseID" });
            DropIndex("public.OperationDevices", new[] { "OperationID" });
            DropIndex("public.OperationHabitMappings", new[] { "HabitID" });
            DropIndex("public.OperationHabitMappings", new[] { "OperationID" });
            DropTable("public.ScenarioAreaBases");
            DropTable("public.ScenarioActors");
            DropTable("public.HabitActors");
            DropTable("public.Scenarios");
            DropTable("public.GraphObjects");
            DropTable("public.GraphNodeDeviceMappings");
            DropTable("public.DeviceBases");
            DropTable("public.AreaBases");
            DropTable("public.OperationDevices");
            DropTable("public.Operations");
            DropTable("public.OperationHabitMappings");
            DropTable("public.Habits");
            DropTable("public.Actor");
        }
    }
}
