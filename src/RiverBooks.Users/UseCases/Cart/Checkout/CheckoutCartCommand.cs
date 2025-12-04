using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.UseCases.Cart.AddItem;

public record CheckoutCartCommand(string EmailAddress, 
                                  Guid shippingAddressId, 
                                  Guid billingAddressId)
                                                          : IRequest<Result<Guid>>;
