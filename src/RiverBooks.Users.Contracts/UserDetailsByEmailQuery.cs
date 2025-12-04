using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.Contracts;

public record UserDetailsByEmailQuery(string EmailAddress) : IRequest<Result<UserDetailsResponse>>;


