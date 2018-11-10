using System;
using System.IO;

namespace AlarmClock.Tools
{
    internal static class FileFolderHelper
    {
        private static readonly string AppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string ClientFolderPath =
            Path.Combine(AppDataPath, "AlarmClock");
        internal static readonly string LogFolderPath =
            Path.Combine(ClientFolderPath, "Log");
        internal static readonly string LogFilepath = Path.Combine(LogFolderPath,
            "App_" + DateTime.Now.ToString("YYYY_MM_DD") + ".txt");
        internal static readonly string DataFolderPath = 
            Path.Combine(ClientFolderPath, "Data");
        internal static readonly string DataFilePath =
            Path.Combine(DataFolderPath, "data.xml");
        internal static readonly string AssetsFolderPath =
            Path.Combine(Environment.CurrentDirectory, @"Assets\");
        internal static readonly string SoundsFolderPath =
            Path.Combine(AssetsFolderPath, "Sounds");
        internal static readonly string AlarmSoundFilepath =
            Path.Combine(SoundsFolderPath, "alarm_sound.wav");
        internal static readonly string LastUserFilePath =
            Path.Combine(ClientFolderPath, "LastUser.alarmclock");

        /// <summary>
        /// Return false if file wasn't exist before
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static bool CheckAndCreateFile(string filePath)
        {
            bool isExistBefore = true;
            try
            {
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
            }
            catch (Exception)
            {
                throw;
            }

            return isExistBefore;
        }
    }
}
