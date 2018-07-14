using ErrorConsole.Core.DomainEvents;
using System;
using System.Security.Cryptography;

namespace ErrorConsole.Core.Models
{
    public class User
    {
        public User(Guid userId)
        {
            UserId = userId;
        }

        public User()
        {
            Salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(Salt);
            }
        }

        public User Reduce(User user, object[] events) {

            foreach(var @event in events)
            {
                switch (@event)
                {
                    case UserCreatedEvent created:
                        user.Password = created.Password;
                        user.Salt = created.Salt;
                        user.Username = created.Username;
                        user.UserId = created.UserId;
                        break;
                }
            }
            return user;
        }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; private set; }
    }
}
