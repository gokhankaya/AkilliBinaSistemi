namespace DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemoryAlanlariEklendi : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Memories", "Definition", c => c.String());
            AddColumn("public.Memories", "ActionName", c => c.String());
            AddColumn("public.Memories", "ActionValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Memories", "ActionValue");
            DropColumn("public.Memories", "ActionName");
            DropColumn("public.Memories", "Definition");
        }
    }
}
