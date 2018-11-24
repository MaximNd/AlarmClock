using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DBModels;

namespace DBAdapter
{
    public static class EntityWrapper
    {
        public static bool UserExists(string login)
        {
            using (var context = new AlarmClockDBContext())
            {
                return context.Users.Any(u => u.Login == login);
            }
        }

        public static User GetUserByLogin(string login)
        {
            using (var context = new AlarmClockDBContext())
            {
                return context.Users.Include(u => u.AlarmClocks).FirstOrDefault(u => u.Login == login);
            }
        }

        public static User GetUserByGuid(Guid guid)
        {
            using (var context = new AlarmClockDBContext())
            {
                return context.Users.Include(u => u.AlarmClocks).FirstOrDefault(u => u.Guid == guid);
            }
        }

        public static List<User> GetAllUsers(Guid alarmClockGuid)
        {
            using (var context = new AlarmClockDBContext())
            {
                return context.Users.Where(u => u.AlarmClocks.All(r => r.Guid != alarmClockGuid)).ToList();
            }
        }

        public static List<User> GetUsers()
        {
            using (var context = new AlarmClockDBContext())
            {
                return context.Users.Include(u => u.AlarmClocks).ToList();
            }
        }

        public static void AddUser(User user)
        {
            using (var context = new AlarmClockDBContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void AddAlarmClock(AlarmClock alarmClock)
        {
            using (var context = new AlarmClockDBContext())
            {
                alarmClock.DeleteDatabaseValues();
                context.AlarmClocks.Add(alarmClock);
                context.SaveChanges();
            }
        }

        public static void SaveAlarmClock(AlarmClock alarmClock)
        {
            using (var context = new AlarmClockDBContext())
            {
                context.Entry(alarmClock).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void DeleteAlarmClock(AlarmClock selectedAlarmClock)
        {
            using (var context = new AlarmClockDBContext())
            {
                selectedAlarmClock.DeleteDatabaseValues();
                context.AlarmClocks.Attach(selectedAlarmClock);
                context.AlarmClocks.Remove(selectedAlarmClock);
                context.SaveChanges();
            }
        }
    }
}
