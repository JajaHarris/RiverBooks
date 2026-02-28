using Ardalis.Result;
using Mediator;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.UseCases.User.GetById;

namespace RiverBooks.Users.Integrations;

public class UserDetailsByIdQueryHandler : IRequestHandler<UserDetailsByIdQuery,
                                                        Result<UserDetailsResponse>>
{
  private readonly IMediator _mediator;

  public UserDetailsByIdQueryHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async ValueTask<Result<UserDetailsResponse>> Handle(UserDetailsByIdQuery request, 
    CancellationToken ct)
  {
    var query = new GetUserByIdQuery(request.UserId);

    var result = await _mediator.Send(query);

    if (result.Status != ResultStatus.Ok) { return Result.NotFound(); }

    var response = new UserDetailsResponse(result.Value.UserId, result.Value.EmailAddress);

    return response;
  }
}
