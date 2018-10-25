using AlarmClock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmClock.Managers
{
    public class DBManager
    {
        private static readonly List<User> Users = new List<User>();

        static DBManager()
        {
            Users.Add(new User("Petro", "Petrenko", "petro@mail.com", "test", "test"));
            Users.Add(new User("Ivan", "Ivanenko", "ivan@mail.com", "test1", "test1"));
        }

        public static bool UserExists(string login)
        {
            return Users.Any(u => u.Login == login);
        }

        public static User GetUserByLogin(string login)
        {
            List<User> temp = Users;
            return Users.FirstOrDefault(u => u.Login == login);
        }

        public static void AddUser(User user)
        {
            Users.Add(user);
        }
    }
}
