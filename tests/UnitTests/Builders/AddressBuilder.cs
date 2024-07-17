using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.UnitTests.Builders;

public class AddressBuilder
{
    private Address _address;

    public string TestStreet => "Ataturk St.";

    public string TestCity => "Izmir";

    public string TestState => "35";

    public string TestCountry => "Turkiye";

    public string TestZipCode => "35530";

    public AddressBuilder()
    {
        _address = WithDefaultValues();
    }
    public Address Build()
    {
        return _address;
    }
    public Address WithDefaultValues()
    {
        _address = new Address(TestStreet, TestCity, TestState, TestCountry, TestZipCode);

        return _address;
    }
}
