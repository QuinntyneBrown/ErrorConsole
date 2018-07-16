using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using MediatR;
using System;
using System.Security.Cryptography;

namespace ErrorConsole.Core.Models
{
    public class User: AggregateRoot
    {
        public User(Guid userId, string username = null, byte[] salt= null, string password = null) {
            Apply(new UserCreated()
            {
                UserId = userId,
                Username = username,
                Password = password,
                Salt = salt
            });
        }

        public User()
            :this(Guid.NewGuid())
        { }
        
        public static User Load(Guid userId, INotification[] events)
        {
            var user = new User();

            foreach(var @event in events)
            {
                user.Apply(@event);
            }
            
            return user;
        }



        public void Apply(INotification @event)
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

            RaiseDomainEvent(@event);
        }
        public User Create(UserCreated @event)
        {
            return new User()
            {
                Password = @event.Password,
                Salt = @event.Salt,
                UserId = @event.UserId
            };            
        }

        public User Reduce(User user, object[] events) {

            foreach(var @event in events)
            {
                switch (@event)
                {
                    case UserCreated created:
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
