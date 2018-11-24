using System;

namespace Tools.Serialization
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
    }
}
