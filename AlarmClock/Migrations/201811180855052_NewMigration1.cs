namespace DBAdapter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AlarmClocks", "LastTriggerDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AlarmClocks", "LastTriggerDate", c => c.DateTime(nullable: false));
        }
    }
}
