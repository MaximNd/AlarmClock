using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlarmClock.Models;
using AlarmClock.ViewModels.AlarmClocks.AlarmClock;
using DBModels;
using Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Tools;

namespace Tests
{
    [TestClass]
    public class AlarmClockConfigTests
    {
        #region Fields
        private List<User> users = new List<User>();
        #endregion

        #region Constructor
        public AlarmClockConfigTests()
        {
            users = UserGenerator.GenerateUsers();
            StationManager.CurrentUser = users[1];
        }
        #endregion

        [TestMethod]
        public void CheckExistingOfAlarmClock()
        {
            DateTime now = DateTime.Now;
            AlarmClockForView alarmClockForView = new AlarmClockForView(null, now);
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClockForView);
            
            Assert.IsTrue(alarmClockViewModel.IsClockPresent);
            Assert.AreSame(alarmClockViewModel.CurrentAlarmClock.AlarmClock, alarmClockForView.AlarmClock);

            alarmClockViewModel.CurrentAlarmClock = null;
            Assert.IsFalse(alarmClockViewModel.IsClockPresent);
            Assert.IsNull(alarmClockViewModel.CurrentAlarmClock);

            AlarmClockForView alarmClockForView2 = new AlarmClockForView(null, now);
            AlarmClockViewModel alarmClockViewModel2 = new AlarmClockViewModel(null);

            Assert.IsFalse(alarmClockViewModel2.IsClockPresent);
            Assert.IsNull(alarmClockViewModel2.CurrentAlarmClock);
            

            alarmClockViewModel2.CurrentAlarmClock = alarmClockForView2;

            Assert.IsTrue(alarmClockViewModel2.IsClockPresent);
            Assert.AreSame(alarmClockViewModel2.CurrentAlarmClock.AlarmClock, alarmClockForView2.AlarmClock);
        }

        [TestMethod]
        public void CheckUniquenessOfNewAlarmClock()
        {
            AlarmClockForView alarmClockForView = new AlarmClockForView(StationManager.CurrentUser.AlarmClocks[0]);
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClockForView);
            Assert.AreSame(alarmClockViewModel.CurrentAlarmClock.AlarmClock, alarmClockForView.AlarmClock);

            PrivateObject obj = new PrivateObject(alarmClockViewModel);
            DateTime newUniqueDateTime = CreateUniqueDateTime();
            DateTime notUniqueDateTime1 = StationManager.CurrentUser.AlarmClocks[0].NextTriggerDate;
            DateTime notUniqueDateTime2 = StationManager.CurrentUser.AlarmClocks[1].NextTriggerDate;

            Assert.AreEqual(notUniqueDateTime1, StationManager.CurrentUser.AlarmClocks[0].NextTriggerDate);
            Assert.AreEqual(notUniqueDateTime2, StationManager.CurrentUser.AlarmClocks[1].NextTriggerDate);

            Assert.IsTrue((bool)obj.Invoke("CheckUniqueness", newUniqueDateTime));
            Assert.IsTrue((bool)obj.Invoke("CheckUniqueness", notUniqueDateTime1));
            Assert.IsFalse((bool)obj.Invoke("CheckUniqueness", notUniqueDateTime2));
        }

        [TestMethod]
        public void CheckStoppingAlarmClock()
        {
            DateTime now = DateTime.Now;
            AlarmClockForView alarmClockForView = new AlarmClockForView(null, now);
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClockForView);

            PrivateObject obj = new PrivateObject(alarmClockViewModel);
            obj.Invoke("SnoozeExecute", (object)null);

            Assert.IsFalse(alarmClockForView.IsAlarming);
        }

        [TestMethod]
        public void TestAlarming()
        {
            DateTime now = DateTime.Now;
            AlarmClockForView alarmClockForView = new AlarmClockForView(null, now);
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClockForView);
            int beforeYear = alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate.Year;
            int beforeMonth = alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate.Month;
            int beforeDay = alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate.Day;
            int beforeHour = alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate.Hour;
            int beforeMinute = alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate.Minute;
            int beforeSecond = alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate.Second;

            alarmClockViewModel.CurrentAlarmClock.Alarm();
            
            Assert.IsTrue(alarmClockViewModel.CurrentAlarmClock.AlarmClock.IsAlarming);
            Assert.IsNotNull(alarmClockViewModel.CurrentAlarmClock.AlarmClock.LastTriggerDate);
            Assert.IsInstanceOfType(alarmClockViewModel.CurrentAlarmClock.AlarmClock.LastTriggerDate, typeof(DateTime));

            DateTime LastTriggerDate = (DateTime) alarmClockViewModel.CurrentAlarmClock.AlarmClock.LastTriggerDate;

            Assert.AreEqual(LastTriggerDate.Year, beforeYear);
            Assert.AreEqual(LastTriggerDate.Month, beforeMonth);
            Assert.AreEqual(LastTriggerDate.Day, beforeDay);
            Assert.AreEqual(LastTriggerDate.Hour, beforeHour);
            Assert.AreEqual(LastTriggerDate.Minute, beforeMinute);
            Assert.AreEqual(LastTriggerDate.Second, beforeSecond);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.AlarmClock.NextTriggerDate, now.AddDays(1));
        }

        private DateTime CreateUniqueDateTime()
        {
            for (int i = 0; i < 59; ++i)
            {
                foreach (var alarmClock in StationManager.CurrentUser.AlarmClocks)
                {
                    if (alarmClock.NextTriggerDate.Minute != i)
                    {
                        var today = DateTime.Today;
                        return new DateTime(today.Year, today.Month, today.Day, today.Hour, i, today.Second);
                    }
                }
            }

            return DateTime.Now;
        }
    }
}
