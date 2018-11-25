using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlarmClock.Models;
using AlarmClock.ViewModels.AlarmClocks;
using DBModels;
using Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Tools;

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
            DBModels.AlarmClock selectedAlarmClock = alarmClocksViewModel.SelectedAlarmClock.AlarmClock;
            Assert.AreEqual(selectedAlarmClock, alarmClocksViewModel.SelectedAlarmClock.AlarmClock);
            int countAlarmClocks = alarmClocksViewModel.AlarmClocks.Count;

            PrivateObject obj = new PrivateObject(alarmClocksViewModel);
            obj.Invoke("DeleteSelectedAlarmClock");

            Assert.AreEqual(countAlarmClocks-1, alarmClocksViewModel.AlarmClocks.Count);
            Assert.AreNotSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, selectedAlarmClock);
            Assert.AreSame(alarmClocksViewModel.SelectedAlarmClock.AlarmClock, alarmClocksViewModel.AlarmClocks[alarmClocksViewModel.AlarmClocks.Count-1].AlarmClock);
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

