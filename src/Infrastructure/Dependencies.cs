using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Infrastructure.Identity;

namespace TicketingApp.Infrastructure;

public static class Dependencies
{
    private const string TICKETINGDB_CONN_STR = "TicketingConnection";
    private const string IDENTITYDB_CONN_STR = "IdentityConnection";

    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddDbContext<TicketingContext>(ticketing =>
            ticketing.UseSqlServer(configuration.GetConnectionString(TICKETINGDB_CONN_STR)));

        services.AddDbContext<AppIdentityDbContext>(ticketing =>
            ticketing.UseSqlServer(configuration.GetConnectionString(IDENTITYDB_CONN_STR)));
    }
}
