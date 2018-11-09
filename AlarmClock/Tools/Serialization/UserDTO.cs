using AlarmClock.Models;
using System;
using System.Linq;

namespace AlarmClock.Tools.Serialization
{
    public class UserDTO
    {
        public string _guid;
        public string _firstName;
        public string _lastName;
        public string _email;
        public string _login;
        public string _password;
        public DateTime _lastLoginDate;
        public AlarmClockDTO[] _alarmClockDTOs;

        public UserDTO() { }

        public static explicit operator UserDTO(User user)
        {
            UserDTO userDTO = new UserDTO
            {
                _guid = user.Guid.ToString(),
                _firstName = user.FirstName,
                _lastName = user.LastName,
                _email = user.Email,
                _login = user.Login,
                _password = user.Password,
                _lastLoginDate = user.LastLoginDate,
                _alarmClockDTOs = new AlarmClockDTO[user.AlarmClocks.Count()]
            };

            for (int i = 0; i < user.AlarmClocks.Count(); ++i)
            {
                userDTO._alarmClockDTOs[i] = new AlarmClockDTO
                {
                    _guid = user.AlarmClocks[i].Guid.ToString(),
                    _lastTriggerDate = user.AlarmClocks[i].LastTriggerDate,
                    _nextTriggerDate = user.AlarmClocks[i].NextTriggerDate
                };
            }
            return userDTO;
        }
    }



}
