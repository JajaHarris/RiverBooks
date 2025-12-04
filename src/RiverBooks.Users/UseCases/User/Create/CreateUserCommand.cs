using Ardalis.Result;
using Mediator;

namespace RiverBooks.Users.UseCases.User.Create;
public record CreateUserCommand(string Email, string Password) : IRequest<Result>;
