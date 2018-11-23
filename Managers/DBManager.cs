using AlarmClock.Models;
using DBAdapter;
using System.Collections.Generic;

namespace AlarmClock.Managers
{
    public class DBManager
    {
        public static List<User> Users = new List<User>();

        static DBManager()
        {
            AddUser(new User("Petro", "Petrenko", "petro@mail.com", "test", "test"));
            AddUser(new User("Ivan", "Ivanenko", "ivan@mail.com", "test1", "test1"));
        }

        public static bool UserExists(string login)
        {
            return EntityWrapper.UserExists(login);
        }

        public static User GetUserByLogin(string login)
        {
            return EntityWrapper.GetUserByLogin(login);
        }

        public static void AddUser(User user)
        {
            if (!UserExists(user.Login))
                EntityWrapper.AddUser(user);
        }

        public static List<User> GetAllUsers()
        {
            return EntityWrapper.GetUsers();
        }

        public static void AddAlarmClock(Models.AlarmClock alarmClock)
        {
            EntityWrapper.AddAlarmClock(alarmClock);
        }

        public static void SaveAlarmClock(Models.AlarmClock alarmClock)
        {
            EntityWrapper.SaveAlarmClock(alarmClock);
        }

        public static void DeleteAlarmClock(Models.AlarmClock selectedAlarmClock)
        {
            EntityWrapper.DeleteAlarmClock(selectedAlarmClock);
        }
    }
}
