using Ardalis.Specification;

namespace TicketingApp.Core.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{

}
