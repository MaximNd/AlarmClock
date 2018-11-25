using System;
using System.IO;
using System.Xml.Serialization;
using DBModels;
using Tools;
using Tools.Serialization;

namespace Managers
{
    public static class StationManager
    {
        private static readonly string DataFile = FileFolderHelper.DataFilePath;

        public static User CurrentUser { get; set; } = null;

        public static void ImportData()
        {
            if (!FileFolderHelper.CheckAndCreateFile(DataFile))
            {
                ExportData();
            }
            TextReader reader = null;
            try
            {
                reader = new StreamReader(DataFile);
                var serializer = new XmlSerializer(typeof(StoredData));
                StoredData dataToStore = (StoredData)serializer.Deserialize(reader);
                CurrentUser = DBManager.GetUserByLogin(dataToStore.currentUserLogin);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static void ExportData()
        {
            User currentUser = CurrentUser;
            
            StoredData dataToStore = new StoredData();
            if (currentUser == null)
                dataToStore.currentUserLogin = "";
            else
                dataToStore.currentUserLogin = currentUser.Login;

            TextWriter writer = null;

            try
            {
                var serializer = new XmlSerializer(typeof(StoredData));
                writer = new StreamWriter(DataFile, false);
                serializer.Serialize(writer, dataToStore);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        public static void CloseApp()
        {
            ExportData();

            Environment.Exit(1);
        }
    }
}
