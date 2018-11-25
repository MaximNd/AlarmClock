using System;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

namespace DBModels
{
    [DataContract(IsReference = true)]
    public class AlarmClock
    {
        #region Fields
        [DataMember]
        private Guid _guid;
        [DataMember]
        private DateTime? _lastTriggerDate;
        [DataMember]
        private DateTime _nextTriggerDate;
        [DataMember]
        private bool _isAlarming;
        [DataMember]
        private Guid _userGuid;
        [DataMember]
        private User _user;
        #endregion

        #region Fields
        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        public DateTime? LastTriggerDate
        {
            get { return _lastTriggerDate; }
            set { _lastTriggerDate = value; }
        }

        public DateTime NextTriggerDate
        {
            get { return _nextTriggerDate; }
            set
            {
                _nextTriggerDate = normalizeDate(value);
                
            }
        }

        public bool IsAlarming
        {
            get { return _isAlarming; }
            set { _isAlarming = value; }
        }

        public Guid UserGuid
        {
            get { return _userGuid; }
            set { _userGuid = value; }
        }
        
        public User User
        {
            get { return _user; }
            private set { _user = value; }
        }
        #endregion

        #region Constructor
        public AlarmClock(DateTime? lastTriggerDate, DateTime nextTriggerDate)
        {
            _guid = Guid.NewGuid();
            _lastTriggerDate = lastTriggerDate;
            _nextTriggerDate = normalizeDate(nextTriggerDate);
            _isAlarming = false;
        }
        public AlarmClock()
        {
            _guid = Guid.NewGuid();
        }
        #endregion


        private DateTime normalizeDate(DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            if (dateTime <= now)
            {
                return dateTime.AddDays((now - dateTime).Days + 1);
            }
            if (dateTime > now.AddDays(1))
                return dateTime.AddDays(-(dateTime - now).Days);
            return dateTime;
        }

        public bool Equals(AlarmClock alarmClock)
        {
            return this.NextTriggerDate == alarmClock.NextTriggerDate;
        }

        public override string ToString()
        {
            return $"The alarm clock will trigger at {_nextTriggerDate}";
        }

        #region EntityFrameworkConfiguration
        public class AlarmClockEntityConfiguration : EntityTypeConfiguration<AlarmClock>
        {
            public AlarmClockEntityConfiguration()
            {
                ToTable("AlarmClocks");
                HasKey(s => s.Guid);

                Property(p => p.Guid)
                    .HasColumnName("Guid")
                    .IsRequired();
                Property(p => p.LastTriggerDate)
                    .HasColumnName("LastTriggerDate");
                Property(s => s.NextTriggerDate)
                    .HasColumnName("NextTriggerDate")
                    .IsRequired();
                Ignore(p => p.IsAlarming);
            }
        }
        #endregion

        public void DeleteDatabaseValues()
        {
            _user = null;
        }
    }
}
