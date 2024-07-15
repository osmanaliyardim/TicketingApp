using Ardalis.Specification;

namespace TicketingApp.ApplicationCore.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{

}