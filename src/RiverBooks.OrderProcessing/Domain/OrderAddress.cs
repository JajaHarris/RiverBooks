namespace RiverBooks.OrderProcessing.Domain;

// This is the materialized view's data model
public record OrderAddress(Guid Id, Address Address);
