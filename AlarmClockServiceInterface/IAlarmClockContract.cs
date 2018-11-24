using System;
using System.Collections.Generic;
using System.ServiceModel;
using DBModels;

namespace AlarmClockServiceInterface
{
    [ServiceContract]
    public interface IAlarmClockContract
    {
        [OperationContract]
        bool UserExists(string login);
        [OperationContract]
        User GetUserByLogin(string login);
        [OperationContract]
        User GetUserByGuid(Guid guid);
        [OperationContract]
        List<User> GetAllUsers(Guid alarmClockGuid);
        [OperationContract]
        void AddUser(User user);
        [OperationContract]
        void AddAlarmClock(AlarmClock alarmClock);
        [OperationContract]
        void SaveAlarmClock(AlarmClock alarmClock);
        [OperationContract]
        void DeleteAlarmClock(AlarmClock selectedAlarmClock);
    }
}
