namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyNameChanged: DomainEvent
    {
        public CompanyNameChanged(string name) => Name = name;

        public string Name { get; set; }
    }
}
