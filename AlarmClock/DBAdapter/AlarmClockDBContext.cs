using System.Data.Entity;
using AlarmClock.Models;
using DBAdapter.Migrations;
using AlarmClockModel = AlarmClock.Models.AlarmClock;

namespace DBAdapter
{
    class AlarmClockDBContext : DbContext
    {

        public AlarmClockDBContext() : base("NewAlarmClockDB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AlarmClockDBContext, Configuration>(true));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AlarmClockModel> AlarmClocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new User.UserEntityConfiguration());
            modelBuilder.Configurations.Add(new AlarmClockModel.AlarmClockEntityConfiguration());
        }
    }
}
