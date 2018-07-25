namespace ErrorConsole.Core.DomainEvents
{
    public class DashboardCreated: DomainEvent
    {
        public DashboardCreated(string name) => Name = name;
        public string Name { get; set; }
    }
}
