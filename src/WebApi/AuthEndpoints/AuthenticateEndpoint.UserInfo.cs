﻿namespace TicketingApp.WebApi.AuthEndpoints;

public class UserInfo
{
    public static readonly UserInfo Anonymous = new UserInfo();

    public bool IsAuthenticated { get; set; }

    public string NameClaimType { get; set; } = string.Empty;

    public string RoleClaimType { get; set; } = string.Empty;

    public IEnumerable<ClaimValue> Claims { get; set; } = new List<ClaimValue>();
}
