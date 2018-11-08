using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using AlarmClock.Models;
using AlarmClock.Tools.Serialization;

namespace AlarmClock.Managers
{
    public static class StationManager
    {
        private static readonly string dataFile = "../../data.xml";

        public static User CurrentUser { get; set; }

        public static void ImportData()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Tools.StoredData));
                reader = new StreamReader(dataFile);
                Tools.StoredData dataToStore = (Tools.StoredData)serializer.Deserialize(reader);
                DBManager.Users = new List<User>(Array.ConvertAll(dataToStore.users, user => (User)user));
                CurrentUser = DBManager.Users.FirstOrDefault(u => u.Login.Equals(dataToStore.currentUserLogin));
                User currentUser = CurrentUser;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static void ExportData()
        {
            List<User> allUsers = DBManager.Users;
            User currentUser = CurrentUser;

            if (currentUser != null)
            {
                allUsers.Remove(allUsers.Find(u => u.Login.Equals(currentUser.Login)));
                allUsers.Add(currentUser);
            }

            Tools.StoredData dataToStore = new Tools.StoredData
            {
                users = Array.ConvertAll(allUsers.ToArray(), user => (UserDTO)user)
            };
            if (currentUser == null)
                dataToStore.currentUserLogin = "";
            else
                dataToStore.currentUserLogin = currentUser.Login;

            TextWriter writer = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Tools.StoredData));
                writer = new StreamWriter(dataFile, false);
                serializer.Serialize(writer, dataToStore);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        internal static void CloseApp()
        {
            ExportData();

            MessageBox.Show("All Data Saved.\nShutDown");
            Environment.Exit(1);
        }
    }
}
