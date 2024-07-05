using Ardalis.Specification;

namespace TicketingApp.Core.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{

}