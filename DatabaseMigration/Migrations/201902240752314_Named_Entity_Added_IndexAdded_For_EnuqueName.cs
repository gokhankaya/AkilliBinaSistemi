namespace DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Named_Entity_Added_IndexAdded_For_EnuqueName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.Areas", "Name", c => c.String(maxLength: 450));
            AlterColumn("public.AreaTypes", "Name", c => c.String(maxLength: 450));
            AlterColumn("public.Items", "Name", c => c.String(maxLength: 450));
            CreateIndex("public.Areas", "Name", unique: true);
            CreateIndex("public.AreaTypes", "Name", unique: true);
            CreateIndex("public.Items", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("public.Items", new[] { "Name" });
            DropIndex("public.AreaTypes", new[] { "Name" });
            DropIndex("public.Areas", new[] { "Name" });
            AlterColumn("public.Items", "Name", c => c.String());
            AlterColumn("public.AreaTypes", "Name", c => c.String());
            AlterColumn("public.Areas", "Name", c => c.String());
        }
    }
}
