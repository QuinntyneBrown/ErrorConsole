using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using MediatR;
using System;

namespace ErrorConsole.Core.Models
{
    public class DigitalAsset : AggregateRoot
    {
        public DigitalAsset()
        {

        }

        public DigitalAsset(Guid digitalAssetId, string name, byte[] bytes, string contentType)
        {
            Apply(new DigitalAssetCreated(digitalAssetId, name, bytes, contentType));
        }
        public Guid DigitalAssetId { get; set; }
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }

        public override void Apply(DomainEvent @event)
        {
            switch(@event)
            {
                case DigitalAssetCreated digitalAssetCreated:
                    DigitalAssetId = digitalAssetCreated.DigitalAssetId;
                    Bytes = digitalAssetCreated.Bytes;
                    ContentType = digitalAssetCreated.ContentType;
                    Name = digitalAssetCreated.Name;
                    break;
            }

            RaiseDomainEvent(@event);
        }
    }
}
