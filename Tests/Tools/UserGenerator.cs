using System;
using System.Collections.Generic;
using AlarmClock.Models;
using AlarmClockModel = AlarmClock.Models.AlarmClock;

namespace Tests.Tools
{
    internal static class UserGenerator
    {
        private static void GenerateAlarmClocks(List<AlarmClockModel> alarmClocks)
        {
            Random random = new Random();
            var today = DateTime.Today;
            for (int i = 0; i < 10; i++)
            {
                alarmClocks.Add(new AlarmClockModel(null, new DateTime(today.Year, today.Month, today.Day, random.Next(0, 23), random.Next(0, 59), 0)));
            }
        }

        public static List<User> GenerateUsers()
        {
            List<User> users = new List<User>();
            users.Add(new User("userFirstName1", "userLastName2", "user1Email@email.com", "login1", "1234"));
            users.Add(new User("userFirstName2", "userLastName1", "user2Email@email.com", "login2", "4321"));
            users.ForEach(user => GenerateAlarmClocks(user.AlarmClocks));
            return users;
        }
    }
}
