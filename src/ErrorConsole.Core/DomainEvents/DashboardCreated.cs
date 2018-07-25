using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class DashboardCreated: DomainEvent
    {
        public DashboardCreated(string name, Guid userId)
        {
            Name = name;
            UserId = userId;
        }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
