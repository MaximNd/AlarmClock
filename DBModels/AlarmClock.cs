using System;
using System.Data.Entity.ModelConfiguration;

namespace DBModels
{
    public class AlarmClock
    {
        #region Fields
        private Guid _guid;
        private DateTime? _lastTriggerDate;
        private DateTime _nextTriggerDate;
        private bool _isAlarming;
        private Guid _userGuid;
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
                _nextTriggerDate = value;
                if (_nextTriggerDate <= DateTime.Now)
                {
                    _nextTriggerDate = _nextTriggerDate.AddDays((DateTime.Now - _nextTriggerDate).Days + 1);
                    return;
                }
                if (_nextTriggerDate > DateTime.Now.AddDays(1))
                    _nextTriggerDate = _nextTriggerDate.AddDays(-(_nextTriggerDate - DateTime.Now).Days);
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
            _nextTriggerDate = nextTriggerDate;
            _isAlarming = false;
        }
        #endregion
        public AlarmClock()
        {
            _guid = Guid.NewGuid();
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
