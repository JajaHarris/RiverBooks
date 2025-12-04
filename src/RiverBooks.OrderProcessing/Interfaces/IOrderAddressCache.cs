using Ardalis.Result;
using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Interfaces;

public interface IOrderAddressCache
{
  Task<Result<OrderAddress>> GetByIdAsync(Guid id);
  Task<Result> StoreAsync(OrderAddress orderAddress);
}
