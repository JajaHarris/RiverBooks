using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.UseCases.Cart.AddItem;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddress) : IRequest<Result>;
