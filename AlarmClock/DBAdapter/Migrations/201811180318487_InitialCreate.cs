namespace DBAdapter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmClocks",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        LastTriggerDate = c.DateTime(nullable: true),
                        NextTriggerDate = c.DateTime(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Guid)
                .ForeignKey("dbo.Users", t => t.UserGuid, cascadeDelete: true)
                .Index(t => t.UserGuid);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Guid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AlarmClocks", "UserGuid", "dbo.Users");
            DropIndex("dbo.AlarmClocks", new[] { "UserGuid" });
            DropTable("dbo.Users");
            DropTable("dbo.AlarmClocks");
        }
    }
}
