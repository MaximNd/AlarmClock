﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Tools;
using Tools.Serialization;

namespace DBModels
{
    [DataContract(IsReference = true)]
    public class User
    {
        #region Const

        private const string PrivateKey =
            "<RSAKeyValue><Modulus>zO+HxK436A49pUWsqK/IWZtH5shNxooQaTWwy53gSsPd5ARg3JlLCoylHTr6sVYtbRUMMr1InMLZtDK1uwh1SUlgw7rHECLB0MsNRN1wrEf5rCtqDNn76EMkrRYIIQ+2Ke8Ff3mHSXhcPQZNgcK5YV7YljEdKeKYB1OlM8Jmvsk=</Modulus><Exponent>AQAB</Exponent><P>/1bVII1govnNGwEQ8V5RwhKthICJt16ZT/Xn5Gc2mQfFiIUguJ26JfHpwei4FG8jB8Xw7vczrTgH6yT2O9LN7w==</P><Q>zXdN8Atku/5qUAOxSYMmVOhRcQ9b0Qrb+he5ZUnbmQWXFXvZS6GNksNoHodQqjFdRPPggBLlTubB9ziT76v2xw==</Q><DP>ZDk1Fr3nfJEIjNzyRYt8E+045pV9eNhM3THsf55zs8V1J4z5tv1SH6rA0jgCaSLmYRq041dslUU09ntfm0O3SQ==</DP><DQ>hDBhoDJ0WM7SLzBw+065dp8Q5qBu/gryg/CHgrcF5WlHTrcjkhkaMHYvopSEPTsNOrN8mGmPxjeISznHU8dbOQ==</DQ><InverseQ>CZN3/p7d2FAtnVAOOHHylZECm0e9ZevEIHHYctpMlRpppysZs+JdUT2m3l891yJ74cJv9PJQ8DGfNN7Tc/CJhg==</InverseQ><D>ITCx9mKY31ZfGYM9QVymwAxsCq5qGjuGCOQPLAr3pmQubZ1f6ppREvZQT3mb3Fiuprn/7b/GIM1V4N9Nm2r1Q4yjWq31LVqkLdjlx/J36uX8eST9ndySMViyKNEBUIEJs6bpaoviC5Z0fCT3x3+oX9tFbGxaFiSdghv0lTzBeTk=</D></RSAKeyValue>";

        private const string PublicKey =
            "<RSAKeyValue><Modulus>zO+HxK436A49pUWsqK/IWZtH5shNxooQaTWwy53gSsPd5ARg3JlLCoylHTr6sVYtbRUMMr1InMLZtDK1uwh1SUlgw7rHECLB0MsNRN1wrEf5rCtqDNn76EMkrRYIIQ+2Ke8Ff3mHSXhcPQZNgcK5YV7YljEdKeKYB1OlM8Jmvsk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        #endregion

        #region Fields
        [DataMember]
        private Guid _guid;
        [DataMember]
        private string _firstName;
        [DataMember]
        private string _lastName;
        [DataMember]
        private string _email;
        [DataMember]
        private string _login;
        [DataMember]
        private string _password;
        [DataMember]
        private DateTime _lastLoginDate;
        [DataMember]
        private List<AlarmClock> _alarmClocks;

        #endregion

        #region Properties

        [XmlIgnore]
        public Guid Guid
        {
            get { return _guid; }
            private set { _guid = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Login
        {
            get { return _login; }
            private set { _login = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public DateTime LastLoginDate
        {
            get { return _lastLoginDate; }
            set { _lastLoginDate = value; }
        }

        public List<AlarmClock> AlarmClocks
        {
            get { return _alarmClocks; }
            private set { _alarmClocks = value; }
        }

        #endregion

        #region Constructor

        public User(string firstName, string lastName, string email, string login, string password)
        {
            _guid = Guid.NewGuid();
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _login = login;
            _lastLoginDate = DateTime.Now;
            _alarmClocks = new List<AlarmClock>();
            SetPassword(password);
        }

        private User()
        {
        }

        #endregion

        private void SetPassword(string password)
        {
            _password = Encrypting.EncryptText(password, PublicKey);
        }

        public bool CheckPassword(string password)
        {
            try
            {
                string res = Encrypting.DecryptString(_password, PrivateKey);
                string res2 = Encrypting.GetMd5HashForString(password);
                return res == res2;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static explicit operator User(UserDTO userDTO)
        {
            User user = new User
            {
                _guid = new Guid(userDTO._guid),
                _firstName = userDTO._firstName,
                _lastName = userDTO._lastName,
                _email = userDTO._email,
                _login = userDTO._login,
                _password = userDTO._password,
                _lastLoginDate = userDTO._lastLoginDate,
                _alarmClocks = new List<AlarmClock>()
            };
            foreach (AlarmClockDTO acDTO in userDTO._alarmClockDTOs)
            {
                user._alarmClocks.Add(new AlarmClock
                {
                    IsAlarming = false,
                    Guid = new Guid(acDTO._guid),
                    LastTriggerDate = acDTO._lastTriggerDate,
                    NextTriggerDate = acDTO._nextTriggerDate
                });
            }

            return user;
        }

        public UserDTO ToDTO()
        {
            UserDTO userDTO = new UserDTO
            {
                _guid = this.Guid.ToString(),
                _firstName = this.FirstName,
                _lastName = this.LastName,
                _email = this.Email,
                _login = this.Login,
                _password = this.Password,
                _lastLoginDate = this.LastLoginDate,
                _alarmClockDTOs = new AlarmClockDTO[this.AlarmClocks.Count()]
            };

            for (int i = 0; i < this.AlarmClocks.Count(); ++i)
            {
                userDTO._alarmClockDTOs[i] = new AlarmClockDTO
                {
                    _guid = this.AlarmClocks[i].Guid.ToString(),
                    _lastTriggerDate = this.AlarmClocks[i].LastTriggerDate,
                    _nextTriggerDate = this.AlarmClocks[i].NextTriggerDate
                };
            }
            return userDTO;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }

        #region EntityConfiguration

        public class UserEntityConfiguration : EntityTypeConfiguration<User>
        {
            public UserEntityConfiguration()
            {
                ToTable("Users");
                HasKey(s => s.Guid);

                Property(p => p.Guid)
                    .HasColumnName("Guid")
                    .IsRequired();
                Property(p => p.FirstName)
                    .HasColumnName("FirstName")
                    .IsRequired();
                Property(p => p.LastName)
                    .HasColumnName("LastName")
                    .IsRequired();
                Property(p => p.Email)
                    .HasColumnName("Email")
                    .IsOptional();
                Property(p => p.Login)
                    .HasColumnName("Login")
                    .IsRequired();
                Property(p => p.Password)
                    .HasColumnName("Password")
                    .IsRequired();
                Property(p => p.LastLoginDate)
                    .HasColumnName("LastLoginDate")
                    .IsRequired();

                HasMany(s => s.AlarmClocks)
                    .WithRequired(ac => ac.User)
                    .HasForeignKey(ac => ac.UserGuid)
                    .WillCascadeOnDelete(true);
            }
        }
        #endregion
    }
}
