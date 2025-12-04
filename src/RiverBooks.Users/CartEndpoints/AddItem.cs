using System.Security.Claims;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using Mediator;
using RiverBooks.Users.UseCases.Cart.AddItem;

namespace RiverBooks.Users.CartEndpoints;
internal class AddItem : Endpoint<AddCartItemRequest>
{
  private readonly IMediator _mediator;

  public AddItem(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post("/cart");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(AddCartItemRequest request,
             CancellationToken cancellationToken = default)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var command = new AddItemToCartCommand(request.BookId, request.Quantity, emailAddress!);

    var result = await _mediator.Send(command);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await HttpContext.Response.SendUnauthorizedAsync();
    }
    if (result.Status == ResultStatus.Invalid)
    {
      await HttpContext.Response.SendResultAsync(result.ToMinimalApiResult());
    }
    else
    {
      await HttpContext.Response.SendOkAsync();
    }
  }
}
