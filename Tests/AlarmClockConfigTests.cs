using System;
using System.Collections.Generic;
using AlarmClock.Managers;
using AlarmClock.Models;
using AlarmClock.ViewModels.AlarmClocks.AlarmClock;
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
            DBManager.Users = users;
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
        public void CheckUpdatingTimeAlarmClock()
        {
            DateTime now = DateTime.Now;
            AlarmClockForView alarmClockForView = new AlarmClockForView(null, now.AddMinutes(2));
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClockForView);

            PrivateObject obj = new PrivateObject(alarmClockViewModel);
            DateTime newDateTime = now.AddMinutes(4);
            obj.Invoke("UpdateTime", newDateTime);

            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate.Year, newDateTime.Year);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate.Month, newDateTime.Month);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate.Day, newDateTime.Day);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate.Hour, newDateTime.Hour);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate.Minute, newDateTime.Minute);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate.Second, newDateTime.Second);
        }



        [TestMethod]
        public void CheckTriggeringAlarmClock()
        {
            DateTime now = DateTime.Now;
            AlarmClockForView alarmClockForView = new AlarmClockForView(null, now);
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClockForView);

            PrivateObject obj = new PrivateObject(alarmClockViewModel);
            obj.Invoke("TestAlarmExecute", (object) null);

            Assert.IsTrue(alarmClockForView.IsAlarming);
            Assert.AreEqual(alarmClockViewModel.CurrentAlarmClock.NextTriggerDate, now.AddDays(1));
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
