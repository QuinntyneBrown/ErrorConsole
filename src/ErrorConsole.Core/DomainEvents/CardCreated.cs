namespace ErrorConsole.Core.DomainEvents
{
    public class CardCreated: DomainEvent
    {
        public CardCreated(string name) => Name = name;
        public string Name { get; set; }
    }
}
