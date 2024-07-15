﻿using Ardalis.Specification.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    public EfRepository(TicketingContext dbContext) : base(dbContext)
    {

    }
}
