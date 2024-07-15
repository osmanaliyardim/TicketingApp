using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Constants;

namespace TicketingApp.Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {

        if (identityDbContext.Database.IsSqlServer())
        {
            identityDbContext.Database.Migrate();
        }

        await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.ADMINISTRATORS));
        await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.EVENT_MANAGERS));
        await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.CUSTOMERS));

        // Add Event Manager User
        string eventManagerUserName = "eventmanager@ticketing.com";
        var defaultEventManagerUser = new ApplicationUser { UserName = eventManagerUserName, Email = eventManagerUserName };
        await userManager.CreateAsync(defaultEventManagerUser, AuthorizationConstants.DEFAULT_PASSWORD);

        defaultEventManagerUser = await userManager.FindByNameAsync(eventManagerUserName);
        if (defaultEventManagerUser != null)
        {
            await userManager.AddToRoleAsync(defaultEventManagerUser, AuthorizationConstants.EVENT_MANAGERS);
        }

        // Add Customer User
        string customerUserName = "customer@ticketing.com";
        var defaultCustomerUser = new ApplicationUser { UserName = customerUserName, Email = customerUserName };
        await userManager.CreateAsync(defaultCustomerUser, AuthorizationConstants.DEFAULT_PASSWORD);

        defaultCustomerUser = await userManager.FindByNameAsync(customerUserName);
        if (defaultCustomerUser != null)
        {
            await userManager.AddToRoleAsync(defaultCustomerUser, AuthorizationConstants.CUSTOMERS);
        }

        // Add Admin User
        string adminUserName = "admin@ticketing.com";
        var defaultAdminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
        await userManager.CreateAsync(defaultAdminUser, AuthorizationConstants.DEFAULT_PASSWORD);

        defaultAdminUser = await userManager.FindByNameAsync(adminUserName);
        if (defaultAdminUser != null)
        {
            await userManager.AddToRoleAsync(defaultAdminUser, AuthorizationConstants.ADMINISTRATORS);
        }
    }
}
