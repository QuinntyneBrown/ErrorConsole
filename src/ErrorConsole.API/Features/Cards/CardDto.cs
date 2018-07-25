using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Cards
{
    public class CardDto
    {        
        public Guid CardId { get; set; }
        public string Name { get; set; }

        public static CardDto FromCard(Card card)
            => new CardDto
            {
                CardId = card.CardId,
                Name = card.Name
            };
    }
}
