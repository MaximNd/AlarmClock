using System;
using System.Collections.Generic;
using System.ServiceModel;
using DBModels;

namespace AlarmClockServiceInterface
{
    public class AlarmClockServiceWrapper
    {
        public static bool UserExists(string login)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                return client.UserExists(login);
            }
        }

        public static User GetUserByLogin(string login)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                return client.GetUserByLogin(login);
            }
        }

        public static User GetUserByGuid(Guid guid)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                return client.GetUserByGuid(guid);
            }
        }

        public static List<User> GetAllUsers()
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                return client.GetAllUsers();
            }
        }

        public static void AddUser(User user)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                client.AddUser(user);
            }
        }

        public static void AddAlarmClock(AlarmClock alarmClock)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                client.AddAlarmClock(alarmClock);
            }
        }

        public static void SaveAlarmClock(AlarmClock alarmClock)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                client.SaveAlarmClock(alarmClock);
            }
        }

        public static void DeleteAlarmClock(AlarmClock selectedAlarmClock)
        {
            using (var myChannelFactory = new ChannelFactory<IAlarmClockContract>("Alarm Clock Service"))
            {
                IAlarmClockContract client = myChannelFactory.CreateChannel();
                client.DeleteAlarmClock(selectedAlarmClock);
            }
        }
    }
}
