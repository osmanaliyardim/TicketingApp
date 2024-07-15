using System.Runtime.Serialization;

namespace TicketingApp.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
    public EmptyBasketOnCheckoutException() : base($"Basket cannot have 0 items on checkout")
    {

    }

    protected EmptyBasketOnCheckoutException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {

    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {

    }
}
