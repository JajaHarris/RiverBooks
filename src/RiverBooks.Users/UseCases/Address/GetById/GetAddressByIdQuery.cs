using Ardalis.Result;
using Mediator;
using RiverBooks.Users.UserEndpoints;

namespace RiverBooks.Users.UseCases.Addresses.GetById;

public record GetAddressByIdQuery(Guid AddressId) : IRequest<Result<UserAddressDto>>;
