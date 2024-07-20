using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketingApp.ApplicationCore.Interfaces;
using Ardalis.Specification;

namespace TicketingApp.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    private readonly TicketingContext _context;

    public EfRepository(TicketingContext dbContext) : base(dbContext)
    {

    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}
