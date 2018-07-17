using MediatR;

namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyNameChanged: INotification
    {
        public CompanyNameChanged(string name) => Name = name;

        public string Name { get; set; }
    }
}
