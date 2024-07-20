using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TicketingApp.ApplicationCore.Interfaces;
using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

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

    private async Task SaveChangesWithConcurrencyHandlingAsync()
    {
        bool saveFailed;
        do
        {
            saveFailed = false;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                saveFailed = true;

                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Order)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = await entry.GetDatabaseValuesAsync();

                        if (databaseValues == null)
                        {
                            throw new Exception("The entity being updated has been deleted by another user.");
                        }

                        // Option 1: Refresh the original values to override the client changes
                        entry.OriginalValues.SetValues(databaseValues);

                        // Option 2: Keep the current values and update the database
                        // proposedValues.SetValues(databaseValues);
                    }
                }
            }
        } while (saveFailed);
    }
}
