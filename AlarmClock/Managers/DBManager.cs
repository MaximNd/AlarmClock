using AlarmClock.Models;
using System.Collections.Generic;
using System.Linq;

namespace AlarmClock.Managers
{
    public class DBManager
    {
        internal static List<User> Users = new List<User>();

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
            //TODO? handling when adding user with same login
            if(!UserExists(user.Login))
                Users.Add(user);
        }
    }
}
