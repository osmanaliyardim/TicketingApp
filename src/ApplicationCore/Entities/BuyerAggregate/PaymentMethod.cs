﻿using Ardalis.GuardClauses;

namespace TicketingApp.ApplicationCore.Entities.BuyerAggregate;

public class PaymentMethod : BaseEntity
{
    public string? Alias { get; private set; }

    public string CardId { get; private set; } // actual card data must be stored in a PCI compliant system, like Stripe

    public string Last4 { get; private set; }

    public PaymentMethod(string alias, string cardId, string last4)
    {
        Guard.Against.NullOrEmpty(cardId, nameof(cardId));
        Guard.Against.NullOrEmpty(last4, nameof(last4));

        Alias = alias;
        CardId = cardId;
        Last4 = last4;
    }
}
