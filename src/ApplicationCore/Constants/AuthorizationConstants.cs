﻿namespace TicketingApp.ApplicationCore.Constants;

public class AuthorizationConstants
{
    public const string AUTH_KEY = "AuthKeyOfDoomThatMustBeAMinimumNumberOfBytes";

    // TODO: Don't use this in production
    public const string DEFAULT_PASSWORD = "Pass@word1";

    // TODO: Change this to an environment variable
    public const string JWT_SECRET_KEY = "SecretKeyOfDoomThatMustBeAMinimumNumberOfBytes";

    // Roles
    public const string ADMINISTRATORS = "Administrators";
    public const string EVENT_MANAGERS = "EventManager";
    public const string CUSTOMERS = "Customer";
}
