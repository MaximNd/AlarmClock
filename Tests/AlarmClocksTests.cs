using System;
using System.Collections.Generic;
using AlarmClock.Managers;
using AlarmClock.Models;
using AlarmClock.ViewModels.AlarmClocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Tools;
using AlarmClockModel = AlarmClock.Models.AlarmClock;

namespace Tests
{
    [TestClass]
    public class AlarmClocksTests
    {
        #region Fields
        private List<User> users = new List<User>();
        #endregion

        #region Constructor
        public AlarmClocksTests()
        {
            users = UserGenerator.GenerateUsers();
            DBManager.Users = users;
            StationManager.CurrentUser = users[1];
        }
        #endregion
        
        [TestMethod]
        public void CheckCountOfAlarmClocks()
        {
            AlarmClocksViewModel alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) => {});

            int countClocks = StationManager.CurrentUser.AlarmClocks.Count;
            Assert.AreEqual(countClocks, alarmClocksViewModel.AlarmClocks.Count);
        }

        [TestMethod]
        public void CheckEqualityOfAlarmClocks()
        {
            AlarmClocksViewModel alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) => { });

            for (int i = 0; i < StationManager.CurrentUser.AlarmClocks.Count; ++i)
            {
                Assert.AreSame(StationManager.CurrentUser.AlarmClocks[i], alarmClocksViewModel.AlarmClocks[i].AlarmClock);
            }
        }

        [TestMethod]
        public void CheckSelectionOfAlarmClock()
        {

            AlarmClocksViewModel alarmClocksViewModel = null;
            AlarmClockForView selectedAlarmClcok = null;
            alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) =>
            {
                if (selectedAlarmClcok != null)
                    Assert.AreSame(selectedAlarmClcok.AlarmClock, alarmClocksViewModel.AlarmClocks[0].AlarmClock);
            });

            selectedAlarmClcok = alarmClocksViewModel.AlarmClocks[0];
            alarmClocksViewModel.SelectedAlarmClock = selectedAlarmClcok;
            Assert.AreSame(selectedAlarmClcok.AlarmClock, alarmClocksViewModel.AlarmClocks[0].AlarmClock);
        }

        [TestMethod]
        public void CheckAdditionOfAlarmClock()
        {
            AlarmClocksViewModel alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) => { });
            int countAlarmClocks = alarmClocksViewModel.AlarmClocks.Count;
            PrivateObject obj = new PrivateObject(alarmClocksViewModel);
            AlarmClockForView newAlarmClock = new AlarmClockForView(null, new DateTime());
            obj.Invoke("AddNewAlarmClock", newAlarmClock);
            Assert.AreEqual(countAlarmClocks + 1, alarmClocksViewModel.AlarmClocks.Count);
            Assert.AreSame(newAlarmClock.AlarmClock, alarmClocksViewModel.AlarmClocks[alarmClocksViewModel.AlarmClocks.Count-1].AlarmClock);
            Assert.AreSame(newAlarmClock.AlarmClock, StationManager.CurrentUser.AlarmClocks[StationManager.CurrentUser.AlarmClocks.Count-1]);
            Assert.AreSame(
                StationManager.CurrentUser.AlarmClocks[StationManager.CurrentUser.AlarmClocks.Count - 1],
                alarmClocksViewModel.AlarmClocks[alarmClocksViewModel.AlarmClocks.Count - 1].AlarmClock
            );
            Assert.AreSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, newAlarmClock.AlarmClock);
            Assert.AreSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, StationManager.CurrentUser.AlarmClocks[StationManager.CurrentUser.AlarmClocks.Count - 1]);
            Assert.AreSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, alarmClocksViewModel.AlarmClocks[alarmClocksViewModel.AlarmClocks.Count - 1].AlarmClock);
        }

        [TestMethod]
        public void CheckDeletionOfAlarmClock()
        {
            AlarmClocksViewModel alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) => { });
            alarmClocksViewModel.SelectedAlarmClock = alarmClocksViewModel.AlarmClocks[alarmClocksViewModel.AlarmClocks.Count-1];
            AlarmClockModel selectedAlarmClock = alarmClocksViewModel.SelectedAlarmClock.AlarmClock;
            Assert.AreEqual(selectedAlarmClock, alarmClocksViewModel.SelectedAlarmClock.AlarmClock);
            int countAlarmClocks = alarmClocksViewModel.AlarmClocks.Count;

            PrivateObject obj = new PrivateObject(alarmClocksViewModel);
            obj.Invoke("DeleteSelectedAlarmClock");

            Assert.AreEqual(countAlarmClocks-1, alarmClocksViewModel.AlarmClocks.Count);
            Assert.AreNotSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, selectedAlarmClock);
            Assert.AreSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, alarmClocksViewModel.AlarmClocks[alarmClocksViewModel.AlarmClocks.Count-1].AlarmClock);
        }

        [TestMethod]
        public void TestAlarming()
        {
            AlarmClocksViewModel alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) => { });
            int beforeYear = alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Year;
            int beforeMonth = alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Month;
            int beforeDay = alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Day;
            int beforeHour = alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Hour;
            int beforeMinute = alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Minute;
            int beforeSecond = alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Second;

            PrivateObject obj = new PrivateObject(alarmClocksViewModel);
            obj.Invoke("DoAlarm", alarmClocksViewModel.AlarmClocks[0]);

            Assert.AreSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, alarmClocksViewModel.AlarmClocks[0].AlarmClock);
            Assert.IsTrue(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.IsAlarming);
            Assert.IsNotNull(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.LastTriggerDate);
            Assert.IsInstanceOfType(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.LastTriggerDate, typeof(DateTime));
            DateTime LastTriggerDate = (DateTime) alarmClocksViewModel.SelectedAlarmClock.AlarmClock.LastTriggerDate;
            Assert.AreEqual(LastTriggerDate.Year, beforeYear);
            Assert.AreEqual(LastTriggerDate.Month, beforeMonth);
            Assert.AreEqual(LastTriggerDate.Day, beforeDay);

            Assert.AreEqual(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Year, beforeYear);
            Assert.AreEqual(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Month, beforeMonth);
            Assert.IsTrue(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Day == beforeDay + 1 || alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Day == beforeDay);
            Assert.AreEqual(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Hour, beforeHour);
            Assert.AreEqual(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Minute, beforeMinute);
            Assert.AreEqual(alarmClocksViewModel.SelectedAlarmClock.AlarmClock.NextTriggerDate.Second, beforeSecond);
        }

        [TestMethod]
        public void TestLogout()
        {
            AlarmClocksViewModel alarmClocksViewModel = new AlarmClocksViewModel((AlarmClockForView alarmClock) => { });
            PrivateObject obj = new PrivateObject(alarmClocksViewModel);
            obj.Invoke("LogoutExecute", (object) null);

            Assert.IsNull(alarmClocksViewModel.SelectedAlarmClock);
            Assert.AreEqual(alarmClocksViewModel.AlarmClocks.Count, 0);
            Assert.IsNull(StationManager.CurrentUser);
        }
    }
}

