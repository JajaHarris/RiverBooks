using Mediator;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Users.Domain;

public class LogNewAddressesHandler : INotificationHandler<AddressAddedEvent>
{
  private readonly ILogger<LogNewAddressesHandler> _logger;

  public LogNewAddressesHandler(ILogger<LogNewAddressesHandler> logger)
  {
    _logger = logger;
  }
  public ValueTask Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("[DE Handler]New address added to user {user}: {address}",
      notification.NewAddress.UserId,
      notification.NewAddress.StreetAddress);

    return ValueTask.CompletedTask;
  }
}
