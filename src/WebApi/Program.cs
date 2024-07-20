using Microsoft.OpenApi.Models;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.Infrastructure.Data;
using TicketingApp.Infrastructure.Logging;
using TicketingApp.WebApi;
using TicketingApp.WebApi.Constants;
using TicketingApp.WebApi.Middleware;
using TicketingApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text;
using TicketingApp.ApplicationCore.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicketingApp.Infrastructure.Data.Queries;
using TicketingApp.ApplicationCore.Services;
using TicketingApp.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();

builder.Logging.AddConsole();

TicketingApp.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IBasketQueryService, BasketQueryService>();
builder.Services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ApiConstants.CORS_POLICY,
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(
                "https://localhost:7076", 
                "http://localhost:5068", 
                "http://localhost:31250");
            corsPolicyBuilder.AllowAnyMethod();
            corsPolicyBuilder.AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(ApiConstants.API_VERSION, new OpenApiInfo { Title = ApiConstants.API_NAME, Version = ApiConstants.API_VERSION });
    c.EnableAnnotations();
    c.SchemaFilter<CustomSchemaFilters>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
            });
});

builder.Services.AddMemoryCache();

var app = builder.Build();

app.Logger.LogInformation("WebApi App created...");
app.Logger.LogInformation("Seeding Database(s)...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var ticketingContext = scopedProvider.GetRequiredService<TicketingContext>();
        await TicketingContextSeed.SeedAsync(ticketingContext, app.Logger);

        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
        await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(ApiConstants.CORS_POLICY);

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketingApp V1");
});

app.MapControllers();
app.MapEndpoints();

app.Logger.LogInformation("Api is launching..");
app.Run();

public partial class Program { }