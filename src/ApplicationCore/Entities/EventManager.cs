﻿namespace TicketingApp.ApplicationCore.Entities;

public class EventManager : BaseEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }
}
