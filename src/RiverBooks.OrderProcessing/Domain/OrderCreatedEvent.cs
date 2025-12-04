using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain;

public class OrderCreatedEvent : DomainEventBase
{
  public OrderCreatedEvent(Order order)
  {
    Order = order;
  }

  public Order Order { get; }
}
