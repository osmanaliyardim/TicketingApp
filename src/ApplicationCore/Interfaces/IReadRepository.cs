using Ardalis.Specification;

namespace TicketingApp.ApplicationCore.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{

}
