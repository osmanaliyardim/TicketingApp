using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TicketingApp.Infrastructure;

public static class Dependencies
{
    private const string TICKETINGDB_CONN_STR = "TicketingConnection";

    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddDbContext<TicketingContext>(ticketing =>
            ticketing.UseSqlServer(configuration.GetConnectionString(TICKETINGDB_CONN_STR)));
    }
}
