namespace DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Areas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        AreaID = c.Int(),
                        AreaTypeID = c.Int(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        DeletedBy = c.String(),
                        DeletedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.AreaTypes", t => t.AreaTypeID)
                .ForeignKey("public.Areas", t => t.AreaID)
                .Index(t => t.AreaID)
                .Index(t => t.AreaTypeID);
            
            CreateTable(
                "public.AreaTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Definition = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        DeletedBy = c.String(),
                        DeletedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "public.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Availablity = c.Boolean(nullable: false),
                        IpV4 = c.String(),
                        IpV6 = c.String(),
                        ItemType = c.String(),
                        AreaOfItemID = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        DeletedBy = c.String(),
                        DeletedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Areas", t => t.AreaOfItemID, cascadeDelete: true)
                .Index(t => t.AreaOfItemID);
            
            CreateTable(
                "public.Memories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AreaID = c.Int(),
                        ItemID = c.Int(),
                        Date = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        DeletedBy = c.String(),
                        DeletedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("public.Areas", t => t.AreaID)
                .ForeignKey("public.Items", t => t.ItemID)
                .Index(t => t.AreaID)
                .Index(t => t.ItemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Areas", "AreaID", "public.Areas");
            DropForeignKey("public.Memories", "ItemID", "public.Items");
            DropForeignKey("public.Memories", "AreaID", "public.Areas");
            DropForeignKey("public.Items", "AreaOfItemID", "public.Areas");
            DropForeignKey("public.Areas", "AreaTypeID", "public.AreaTypes");
            DropIndex("public.Memories", new[] { "ItemID" });
            DropIndex("public.Memories", new[] { "AreaID" });
            DropIndex("public.Items", new[] { "AreaOfItemID" });
            DropIndex("public.Areas", new[] { "AreaTypeID" });
            DropIndex("public.Areas", new[] { "AreaID" });
            DropTable("public.Memories");
            DropTable("public.Items");
            DropTable("public.AreaTypes");
            DropTable("public.Areas");
        }
    }
}
