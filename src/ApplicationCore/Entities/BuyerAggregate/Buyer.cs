using Ardalis.GuardClauses;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Entities.BuyerAggregate;

public class Buyer : BaseEntity, IAggregateRoot
{
    public string IdentityGuid { get; set; }

    private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

    public IList<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    public Buyer(string identityGuid)
    {
        Guard.Against.NullOrEmpty(identityGuid, nameof(identityGuid));

        IdentityGuid = identityGuid;
    }

    public Buyer(string identityGuid, List<PaymentMethod> paymentMethods)
    {
        Guard.Against.NullOrEmpty(identityGuid, nameof(identityGuid));
        Guard.Against.NullOrEmpty(paymentMethods, nameof(paymentMethods));

        IdentityGuid = identityGuid;
        _paymentMethods = paymentMethods;
    }
}
