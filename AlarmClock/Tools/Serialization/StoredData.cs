using AlarmClock.Models;
using AlarmClock.Tools.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmClock.Tools
{
    public class StoredData
    {
        public UserDTO[] users;

        public string currentUserLogin;
    }
}
