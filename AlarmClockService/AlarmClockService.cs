using System;
using System.Collections.Generic;
using AlarmClockServiceInterface;
using DBAdapter;
using DBModels;

namespace AlarmClockService
{
    class AlarmClockService : IAlarmClockContract
    {
        public bool UserExists(string login)
        {
            return EntityWrapper.UserExists(login);
        }

        public User GetUserByLogin(string login)
        {
            return EntityWrapper.GetUserByLogin(login);
        }

        public User GetUserByGuid(Guid guid)
        {
            return EntityWrapper.GetUserByGuid(guid);
        }
        public List<User> GetAllUsers()
        {
            return EntityWrapper.GetAllUsers();
        }

        public void AddUser(User user)
        {
            EntityWrapper.AddUser(user);
        }

        public void AddAlarmClock(AlarmClock alarmClock)
        {
            EntityWrapper.AddAlarmClock(alarmClock);
        }

        public void DeleteAlarmClock(AlarmClock selectedAlarmClock)
        {
            EntityWrapper.DeleteAlarmClock(selectedAlarmClock);
        }


        public void SaveAlarmClock(AlarmClock alarmClock)
        {
            EntityWrapper.SaveAlarmClock(alarmClock);
        }
    }
}
