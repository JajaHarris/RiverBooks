using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.Contracts;

public record UserAddressDetailsByIdQuery(Guid AddressId) : IRequest<Result<UserAddressDetails>>;
