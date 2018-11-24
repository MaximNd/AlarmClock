using System;
using System.IO;

namespace Tools
{
    public static class FileFolderHelper
    {
        private static readonly string AppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string ClientFolderPath =
            Path.Combine(AppDataPath, "AlarmClock");
        public static readonly string LogFolderPath =
            Path.Combine(ClientFolderPath, "Log");
        public static readonly string LogFilepath = Path.Combine(LogFolderPath,
            "App_" + DateTime.Now.ToString("YYYY_MM_DD") + ".txt");
        public static readonly string DataFolderPath = 
            Path.Combine(ClientFolderPath, "Data");
        public static readonly string DataFilePath =
            Path.Combine(DataFolderPath, "data.xml");
        public static readonly string AssetsFolderPath =
            Path.Combine(Environment.CurrentDirectory, @"Assets\");
        public static readonly string SoundsFolderPath =
            Path.Combine(AssetsFolderPath, "Sounds");
        public static readonly string AlarmSoundFilepath =
            Path.Combine(SoundsFolderPath, "alarm_sound.wav");
        public static readonly string LastUserFilePath =
            Path.Combine(ClientFolderPath, "LastUser.alarmclock");

        /// <summary>
        /// Return false if file wasn't exist before
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CheckAndCreateFile(string filePath)
        {
            bool isExistBefore = true;
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            if (!file.Exists)
            {
                file.Create().Close();
                isExistBefore = false;
            }

            return isExistBefore;
        }
    }
}
