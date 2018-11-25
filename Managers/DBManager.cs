using System.Collections.Generic;
using AlarmClockServiceInterface;
using DBAdapter;
using DBModels;

namespace Managers
{
    public class DBManager
    {

        public static bool UserExists(string login)
        {
            return AlarmClockServiceWrapper.UserExists(login);
        }

        public static User GetUserByLogin(string login)
        {
            return AlarmClockServiceWrapper.GetUserByLogin(login);
        }

        public static void AddUser(User user)
        {
            if (!UserExists(user.Login))
                AlarmClockServiceWrapper.AddUser(user);
        }

        public static List<User> GetAllUsers()
        {
            return AlarmClockServiceWrapper.GetAllUsers();
        }

        public static void AddAlarmClock(AlarmClock alarmClock)
        {
            AlarmClockServiceWrapper.AddAlarmClock(alarmClock);
        }

        public static void SaveAlarmClock(AlarmClock alarmClock)
        {
            AlarmClockServiceWrapper.SaveAlarmClock(alarmClock);
        }

        public static void DeleteAlarmClock(AlarmClock selectedAlarmClock)
        {
            AlarmClockServiceWrapper.DeleteAlarmClock(selectedAlarmClock);
        }
    }
}
