using Ardalis.Result;
using Mediator;

namespace RiverBooks.Books.Contracts;

public record BookDetailsQuery(Guid BookId) : IRequest<Result<BookDetailsResponse>>;
