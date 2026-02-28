using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.UseCases.User.GetById;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDTO>>;
