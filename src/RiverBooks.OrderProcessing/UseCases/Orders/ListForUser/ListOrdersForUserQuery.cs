using Ardalis.Result;
using Mediator;
using RiverBooks.Users.UseCases;

namespace RiverBooks.OrderProcessing.UseCases.Orders.ListForUser;

public record ListOrdersForUserQuery(string EmailAddress) : IRequest<Result<List<OrderSummary>>>;
