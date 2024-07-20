using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.Storage;

namespace TicketingApp.ApplicationCore.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    // Transaction methods
    Task<IDbContextTransaction> BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}