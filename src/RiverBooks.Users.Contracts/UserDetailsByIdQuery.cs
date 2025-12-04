using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.Contracts;

public record UserDetailsByIdQuery(Guid UserId) : IRequest<Result<UserDetailsResponse>>;


