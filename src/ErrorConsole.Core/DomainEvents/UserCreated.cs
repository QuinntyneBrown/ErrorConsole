using MediatR;
using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class UserCreated: INotification
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
