using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Exceptions;
using System;

namespace ErrorConsole.Core.Models
{
    public class User: AggregateRoot
    {
        public User()
        {

        }

        public User(Guid userId, string username = null, byte[] salt= null, string password = null) 
            => Apply(new UserCreated()
            {
                UserId = userId,
                Username = username,
                Password = password,
                Salt = salt
            });

        protected override void When(DomainEvent @event)
        {
            switch (@event)
            {
                case UserCreated data:
                    UserId = data.UserId;
                    Username = data.Username;
                    Salt = data.Salt;
                    Password = data.Password;
                    break;
            }            
        }

        protected override void EnsureValidState()
        {
            if (UserId == default(Guid))
                throw new AggregateInvalidStateException();
        }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; private set; }
    }
}
