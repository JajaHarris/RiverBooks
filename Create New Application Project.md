# Create New Application Project

RiverBooks is only a reference repository for modeling the structure and architecture of a new application template. It is not a framework, starter kit, shared runtime dependency, or code package that the new application should be built on top of directly. The reference repo can be found at [ardalis/RiverBooks](https://github.com/ardalis/RiverBooks).

Use this document as a build brief for creating a new application that follows the overall RiverBooks architecture while replacing the frontend with Blazor and using Kiota to connect the frontend to a FastEndpoints backend.

## Objective

Create a modular monolith with:

- One deployable backend application
- One Blazor frontend application
- Clear bounded-context modules
- Contracts-only module-to-module coupling
- Per-module persistence boundaries
- FastEndpoints for the HTTP API
- Kiota-generated client SDK consumed by the Blazor app
- Architecture tests that enforce module boundaries
- Aspire-ready local orchestration

This is not a microservices architecture. It is a modular monolith designed so that modules remain internally independent while still shipping as a single system.

## What RiverBooks Is Actually Doing

RiverBooks is best understood as an API-first modular monolith.

- `RiverBooks.Web` is the composition root and runtime host.
- Each business area lives in its own project such as `RiverBooks.Books`, `RiverBooks.Users`, or `RiverBooks.OrderProcessing`.
- Most business modules also expose a separate `*.Contracts` project.
- Cross-cutting primitives live in `RiverBooks.SharedKernel`.
- Local development orchestration lives in `RiverBooks.AppHost` using Aspire.
- Each module registers itself through a `*ModuleServicesExtensions` class.
- Endpoints, use cases, domain model, infrastructure, and integrations are grouped inside each module rather than split into horizontal layers across the whole solution.

## Architectural Methodology to Preserve

### 1. Organize by module, not by technical layer across the entire solution

RiverBooks groups code by bounded context. That is the most important structural decision in the repo.

Use this rule:

- Put all code for a business capability inside its module project.
- Do not create top-level solution folders like `Controllers`, `Services`, `Repositories`, or `Models` that span every feature.

Inside a module, internal layering is still useful, but it stays inside that module.

### 2. Keep one composition root

The host project is responsible for:

- Bootstrapping FastEndpoints
- Configuring authentication/authorization
- Registering shared middleware
- Registering each module through a single extension method
- Publishing OpenAPI
- Hosting the API surface used by Kiota

The host should know about modules. Modules should not know about the host.

### 3. Treat each module as a bounded context

Each module should:

- Own its domain language
- Own its persistence model
- Expose only intentional public contracts
- Avoid direct references to another module's implementation project

In RiverBooks, modules frequently depend on another module's `*.Contracts` project, not its implementation project. Preserve that rule.

### 4. Separate implementation from contracts

For every module that other modules need to talk to, create a matching `*.Contracts` project.

Contracts projects should contain only:

- Request/response DTOs
- Cross-module command/query contracts
- Integration event contracts
- Small shared primitives that are explicitly part of the module's public API

Contracts projects should not contain:

- EF Core types
- Domain entities
- Repositories
- Module service registration
- Endpoint classes

### 5. Favor in-process message-based collaboration between modules

RiverBooks uses mediator-based request/response contracts and domain/integration events to keep modules decoupled.

Preserve this model:

- Use commands for state changes
- Use queries for reads
- Use domain events inside a module
- Use integration events or cross-module request contracts between modules

Do not let one module query another module's database directly.

### 6. Let each module own persistence

RiverBooks uses separate EF Core `DbContext`s per module where needed. Preserve that boundary.

Use these rules:

- Each module has its own data access abstractions.
- Each module may have its own `DbContext`.
- If multiple modules share one physical SQL database, keep separate schemas or at minimum separate `DbContext`s and migration histories.
- No module may depend on another module's infrastructure or database tables directly.

### 7. Keep cross-cutting concerns in a shared kernel, but keep it small

The shared kernel should hold only truly cross-cutting building blocks such as:

- Domain event abstractions
- Pipeline behaviors
- Shared result/error patterns
- Base interfaces for entities that emit domain events

Do not turn the shared kernel into a junk drawer for random helpers.

### 8. Test the architecture, not just behavior

RiverBooks includes architecture tests that guard against infrastructure leakage into domain and use case layers.

Future apps should include automated rules that verify:

- Domain does not depend on infrastructure
- Use cases do not depend on infrastructure
- Modules do not reference other module implementation projects
- Only the host references module implementation projects
- Cross-module references target `*.Contracts` projects only

## Recommended Solution Shape

Use a solution structure like this:

```text
src/
├── MyApp.AppHost/                      # Aspire orchestration
├── MyApp.Api/                          # FastEndpoints host + OpenAPI
├── MyApp.Blazor/                       # Blazor Web App or Blazor WASM host
├── MyApp.Sdk/                          # Kiota-generated client
├── MyApp.SharedKernel/                 # Small cross-cutting primitives
├── MyApp.ServiceDefaults/              # Aspire defaults
├── MyApp.Catalog/
├── MyApp.Catalog.Contracts/
├── MyApp.Identity/
├── MyApp.Identity.Contracts/
├── MyApp.Orders/
├── MyApp.Orders.Contracts/
├── MyApp.Notifications/
├── MyApp.Notifications.Contracts/
├── MyApp.Reporting/
├── MyApp.Reporting.Contracts/          # Optional; only if other modules need it
├── MyApp.Catalog.Tests/
├── MyApp.Orders.Tests/
├── MyApp.ArchitectureTests/
└── Directory.Build.props
```

## Recommended Packages

Use RiverBooks as the reference for package selection, then adapt for the new application's actual needs. Do not add packages just because the reference repo uses them, but these are strong defaults for this style of solution.

### Core API and application packages

- `FastEndpoints`
- `FastEndpoints.Security`
- `FastEndpoints.Swagger`
- `Mediator.Abstractions`
- `Mediator.SourceGenerator`
- `Ardalis.Result`
- `Ardalis.Result.AspNetCore`
- `FluentValidation`

Use these for:

- HTTP endpoint handling
- auth integration
- OpenAPI generation
- mediator-style request/response handling
- result-based application outcomes
- request validation

### Logging and observability

- `Serilog.AspNetCore`
- Aspire service defaults packages used by your app template

Use these for:

- structured logging
- startup and request diagnostics
- telemetry, health checks, and local orchestration defaults

### Persistence and infrastructure

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- your provider package such as `Microsoft.EntityFrameworkCore.SqlServer` or `Npgsql.EntityFrameworkCore.PostgreSQL`

Optional infrastructure packages depending on module needs:

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `StackExchange.Redis`
- `MongoDB.Driver`

### Blazor and client integration

- Blazor packages that ship with the selected .NET template
- Kiota CLI for client generation
- Kiota runtime/auth packages required by the generated C# SDK

Use these for:

- frontend composition
- generated API client creation
- authentication providers and request adapters used by Kiota output

### Testing packages

- `xunit`
- `xunit.runner.visualstudio` or your preferred runner integration
- `Shouldly`
- architecture test tooling such as `ArchUnitNET.xUnit`

Use these for:

- unit tests
- endpoint/use case tests
- architecture boundary tests
- readable assertions

### Example baseline by project type

`MyApp.Api`

- `FastEndpoints`
- `FastEndpoints.Security`
- `FastEndpoints.Swagger`
- `Mediator.Abstractions`
- `Mediator.SourceGenerator`
- `Ardalis.Result`
- `Ardalis.Result.AspNetCore`
- `FluentValidation`
- `Serilog.AspNetCore`
- `Microsoft.AspNetCore.OpenApi`

`MyApp.SharedKernel`

- `Mediator.Abstractions`
- `Ardalis.Result`
- `FluentValidation`

`MyApp.<Module>`

- `Ardalis.Result`
- `FastEndpoints` if the module contains endpoint classes
- `Mediator.Abstractions`
- `Microsoft.EntityFrameworkCore`
- provider-specific EF package when persistence is owned by the module
- `Serilog.AspNetCore` when module-level logging abstractions are needed

`MyApp.Blazor`

- standard Blazor hosting packages from the .NET template
- reference to `MyApp.Sdk`
- Kiota runtime dependencies required by the generated SDK

`MyApp.<Module>.Tests`

- `xunit`
- `Shouldly`
- any test host or integration packages required by the test style

### Package selection rules

- Prefer a small, intentional baseline over a large starter bundle.
- Keep package versions centralized where practical.
- Only add persistence or infrastructure packages to modules that actually need them.
- Keep generated-client dependencies out of backend projects.
- Keep testing-only packages out of production projects.

## Recommended Internal Module Shape

Each module should follow a consistent internal layout:

```text
MyApp.Catalog/
├── CatalogModuleServiceExtensions.cs
├── Domain/
│   ├── Product.cs
│   ├── Money.cs
│   ├── ProductCreatedEvent.cs
│   └── ...
├── Data/
│   ├── CatalogDbContext.cs
│   ├── Configurations/
│   ├── Migrations/
│   └── Repositories/
├── Interfaces/
│   ├── IProductRepository.cs
│   └── IReadOnlyProductRepository.cs
├── UseCases/
│   ├── Products/
│   │   ├── Create/
│   │   ├── UpdatePrice/
│   │   ├── GetById/
│   │   └── List/
│   └── ...
├── Endpoints/
│   ├── Products/
│   │   ├── Create.cs
│   │   ├── GetById.cs
│   │   └── ...
├── Integrations/
│   ├── QueryHandlers/
│   ├── CommandHandlers/
│   └── EventHandlers/
└── AssemblyInfo.cs                     # optional marker for architecture tests
```

Use these intent boundaries:

- `Domain`: entities, value objects, invariants, domain events
- `UseCases`: application logic and orchestrated business flows
- `Endpoints`: HTTP transport adapters only
- `Data` and `Infrastructure`: EF Core, external systems, caches, storage, queues
- `Integrations`: handlers for contracts/events crossing module boundaries
- `Interfaces`: abstractions implemented by infrastructure

## Project Dependency Rules

Enforce these rules strictly.

### Allowed references

- `MyApp.Api` may reference every module implementation project plus `SharedKernel` and `ServiceDefaults`.
- `MyApp.Blazor` may reference `MyApp.Sdk` and UI-specific projects only.
- `MyApp.Sdk` is generated from OpenAPI and should not reference backend implementation projects.
- `MyApp.<Module>` may reference:
  - `MyApp.SharedKernel`
  - Its own `MyApp.<Module>.Contracts`
  - Other modules' `*.Contracts` projects only when required
- `MyApp.<Module>.Contracts` may reference `MyApp.SharedKernel` if needed, but avoid unnecessary dependencies.
- Test projects may reference the project they test and approved test infrastructure.

### Forbidden references

- `MyApp.Blazor` must not reference backend module projects directly.
- `MyApp.<Module>` must not reference another module's implementation project.
- Domain and use case namespaces must not reference infrastructure namespaces.
- One module must not read another module's tables directly.

## RiverBooks Patterns to Keep

### Module self-registration

Each module should expose one registration method:

```csharp
public static class CatalogModuleServiceExtensions
{
    public static IServiceCollection AddCatalogModuleServices(
        this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<Assembly> mediatorAssemblies)
    {
        // module-specific registrations
        // DbContext
        // repositories
        // services
        // external adapters

        mediatorAssemblies.Add(typeof(CatalogModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", "Catalog");
        return services;
    }
}
```

The API host should call one extension method per module and nothing more granular than that.

### Thin endpoints

FastEndpoints classes should stay transport-focused.

Endpoint responsibilities:

- Route configuration
- Auth metadata
- Request binding
- Calling a mediator or application service
- Mapping result to HTTP response

Endpoint non-responsibilities:

- Core business rules
- Multi-step orchestration
- Data access logic

### Use cases as orchestration units

Use cases should handle application flow:

- Validate intent
- Load aggregates through repositories
- Invoke domain behavior
- Persist changes
- Publish or send follow-up messages

### Result pattern for use case outcomes

RiverBooks uses the Result pattern so application and cross-module operations can return structured success, validation, not found, unauthorized, and error outcomes without relying on exceptions for normal control flow.

The reference repo uses the Ardalis packages:

- `Ardalis.Result`
- `Ardalis.Result.AspNetCore`

Use this pattern in new applications for:

- use case handlers
- cross-module command/query handlers
- endpoint-to-use-case response mapping

Typical guidance:

- Return `Result<T>` when a handler produces data
- Return `Result` when a handler only reports success/failure
- Use explicit result states such as invalid, not found, forbidden, unauthorized, or error
- Reserve exceptions for truly exceptional conditions

Example use case handler:

```csharp
using Ardalis.Result;
using Mediator;

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderDto>>;

public sealed class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IOrderReadRepository _orders;

    public GetOrderByIdHandler(IOrderReadRepository orders)
    {
        _orders = orders;
    }

    public async ValueTask<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.NotFound();
        }

        return Result.Success(new OrderDto(order.Id, order.Number, order.Total));
    }
}
```

Example endpoint mapping:

```csharp
public sealed class GetById(IMediator mediator) : Endpoint<GetOrderRequest>
{
    public override void Configure()
    {
        Get("/orders/{id:guid}");
    }

    public override async Task HandleAsync(GetOrderRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetOrderByIdQuery(req.Id), ct);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!result.IsSuccess)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}
```

This keeps the application layer explicit about outcomes and makes endpoint behavior more predictable.

### Domain events for intra-module reactions

Use domain events for business reactions that belong to the same bounded context or are triggered from the same aggregate lifecycle.

Examples:

- `OrderPlaced`
- `InvoiceIssued`
- `UserRegistered`

### Integration contracts for inter-module collaboration

Use integration contracts when one module needs another module's capability or data.

Examples:

- `GetCustomerByIdQuery`
- `ReserveInventoryCommand`
- `InvoicePaidIntegrationEvent`

## How to Adapt the Architecture for Blazor

RiverBooks currently centers on the API host. For new projects, keep that backend structure but introduce a deliberate frontend boundary.

### Frontend rules

- The Blazor app communicates with the backend only through HTTP.
- The Blazor app consumes the generated Kiota SDK, not backend projects.
- UI-specific DTO shaping may happen in the Blazor app, but backend contract types should arrive through the generated SDK.
- Authentication state, routing, forms, and presentation logic stay in the Blazor project.

### Recommended frontend shape

```text
MyApp.Blazor/
├── Components/
├── Pages/
├── Layout/
├── Features/
│   ├── Catalog/
│   ├── Orders/
│   └── Identity/
├── Services/
│   ├── ApiClientFactory.cs
│   ├── CurrentUserAccessor.cs
│   └── ...
├── State/
├── Models/                            # UI-only view models if needed
└── Program.cs
```

Organize the Blazor app by feature where possible, mirroring module names from the backend.

## How to Use Kiota with FastEndpoints

Kiota should be the seam between frontend and backend.

### Principle

- FastEndpoints publishes OpenAPI.
- Kiota generates a typed SDK from that OpenAPI document.
- Blazor uses the SDK.
- Backend modules remain decoupled from the frontend and from generated client concerns.

### Why this is better than sharing backend DTO assemblies with Blazor

- The frontend stays transport-coupled, not implementation-coupled.
- The API remains the contract boundary.
- The generated client reflects the actual HTTP surface.
- Future extraction of modules or replacement of the UI is easier.

### API requirements for Kiota friendliness

Design endpoints so the OpenAPI document is stable and precise.

Use these rules:

- Give requests and responses explicit DTO types.
- Use clear route shapes and versioning strategy.
- Avoid ambiguous polymorphic payloads unless truly needed.
- Ensure authentication and response codes are documented consistently.
- Prefer stable, named DTOs over anonymous response shapes.
- Group endpoints logically by route prefix and tags.

### FastEndpoints/OpenAPI setup guidance

Configure FastEndpoints and Swagger in the API host so the OpenAPI document is suitable for client generation.

Suggested host responsibilities:

- Register FastEndpoints
- Register Swagger/OpenAPI generation
- Publish a stable document name like `v1`
- Keep schema naming consistent
- Include auth scheme metadata

FastEndpoints' Swagger support can generate the document that Kiota consumes, and it supports document naming/tagging plus client-generation-related endpoint mapping if desired. Kiota's official tooling supports generating and updating C# clients from an OpenAPI description. Sources: [FastEndpoints Swagger docs](https://fast-endpoints.com/docs/swagger-support), [Kiota usage docs](https://learn.microsoft.com/en-us/openapi/kiota/using), [Kiota install docs](https://learn.microsoft.com/en-us/openapi/kiota/install).

### Recommended Kiota workflow

Use a dedicated SDK project such as `MyApp.Sdk`.

Suggested flow:

1. Run the API host locally.
2. Export or expose the OpenAPI document at a stable URL.
3. Generate the Kiota client into `src/MyApp.Sdk`.
4. Reference `MyApp.Sdk` from `MyApp.Blazor`.
5. Regenerate the SDK whenever public API contracts change.

### Example generation command

Adjust paths, namespace, and document URL for the new application:

```bash
kiota generate \
  --language csharp \
  --class-name MyAppApiClient \
  --namespace-name MyApp.Sdk \
  --openapi https://localhost:7001/swagger/v1/swagger.json \
  --output ./src/MyApp.Sdk
```

Kiota also supports lock-file based updates, so once generated, future refreshes should prefer an update workflow rather than deleting/regenerating blindly. Source: [Kiota usage docs](https://learn.microsoft.com/en-us/openapi/kiota/using).

### SDK ownership rules

- Treat generated code as generated code.
- Do not hand-edit Kiota output unless there is a documented extension mechanism.
- Put custom wrappers, factories, or auth providers outside generated files.
- If the generated client needs auth or HTTP pipeline customization, implement that in `MyApp.Blazor` or a thin SDK-adapter layer.

## Recommended Runtime Composition

Use these runtime projects:

### `MyApp.Api`

Responsibilities:

- Composition root
- FastEndpoints host
- Auth/authz
- Middleware
- OpenAPI publication
- Module registration

### `MyApp.Blazor`

Responsibilities:

- End-user UI
- Calls backend through `MyApp.Sdk`
- Presents feature workflows
- Holds UI state and client-side composition

### `MyApp.AppHost`

Responsibilities:

- Aspire orchestration for local development
- SQL Server or PostgreSQL container
- Cache container if needed
- Message broker or email test service if needed
- Startup dependencies and environment wiring

### `MyApp.ServiceDefaults`

Responsibilities:

- Shared service defaults for telemetry, health checks, resilience, and service discovery if used

## Persistence Strategy

Default to one `DbContext` per stateful module.

Use this decision matrix:

- If a module owns durable business data, give it its own `DbContext`.
- If a module is read-only or computed, it may only need services and projections.
- If a module integrates with a non-SQL store such as Redis or MongoDB, keep that integration inside the module.

If using one physical database for multiple modules:

- Prefer a separate schema per module
- Keep migrations per module
- Keep connection details centralized in configuration
- Never let one module bypass another module's contracts just because the database is shared

## Suggested Cross-Module Communication Patterns

Use the following preference order.

### 1. Pass required data directly in the command

If one module already has the data needed to complete a workflow, include it in the command instead of forcing additional lookups.

### 2. Use a contracts-based in-process query

If data truly belongs to another module, use a request contract handled by that module.

### 3. Use cached projections only for justified scenarios

Use local caches or materialized views when latency, resiliency, or history requirements justify them.

### 4. Do not perform direct database reads across modules

That is the main boundary violation this architecture is trying to prevent.

## Recommended Initial Module Set for a New Business App

Replace names as needed, but a healthy starting set often looks like:

- `Identity`
- `Catalog` or `ReferenceData`
- `Workflow` or `Orders`
- `Notifications`
- `Reporting`

Do not create modules just because a layer exists. Create modules because a bounded context exists.

## Agent Instructions for Scaffolding a New App

When creating a new project from this blueprint, the agent should follow this order.

### Phase 1: Create the solution skeleton

Create:

- `AppHost`
- `Api`
- `Blazor`
- `Sdk`
- `SharedKernel`
- `ServiceDefaults`
- Initial business modules
- Matching `*.Contracts` projects
- Test projects

### Phase 2: Establish dependency rules

- Add project references that match the rules in this document
- Ensure only `Api` references module implementation projects
- Ensure `Blazor` references only `Sdk` and UI code
- Ensure modules reference only contracts from other modules

### Phase 3: Build the shared infrastructure

- Configure FastEndpoints in `Api`
- Configure authentication
- Configure Swagger/OpenAPI
- Configure mediator pipeline behaviors
- Add shared result and domain event abstractions
- Add logging and health checks
- Add Aspire defaults

### Phase 4: Add module templates

For each module:

- Create registration extension
- Create `Domain`, `UseCases`, `Endpoints`, `Interfaces`, `Data`, and `Integrations` folders
- Add sample command, query, endpoint, repository, and entity
- Add module-specific `DbContext` if needed
- Add a matching `*.Contracts` project

### Phase 5: Create the SDK pipeline

- Expose a stable OpenAPI document
- Generate `MyApp.Sdk` with Kiota
- Wire the generated client into `Blazor`
- Add a regeneration script or documented command

### Phase 6: Add quality gates

- Architecture tests
- Module unit tests
- Endpoint/integration tests
- Build verification for SDK generation if practical

## Concrete Conventions the Agent Should Apply

### Naming

- Solution prefix: `MyApp`
- Module projects: `MyApp.<Module>`
- Public contracts: `MyApp.<Module>.Contracts`
- Host API: `MyApp.Api`
- Frontend: `MyApp.Blazor`
- Generated SDK: `MyApp.Sdk`

### Endpoint style

- One endpoint class per route/action
- Keep endpoint files small
- Pair request DTOs close to their endpoint or use case
- Favor explicit response DTOs

### Use case style

- One folder per use case
- One request type plus one handler
- Validators live next to requests
- Avoid giant "service" classes that absorb every business rule

### Domain style

- Rich entities where invariants matter
- Value objects for business concepts
- Domain events for meaningful business state changes
- Keep persistence concerns out of the domain model where practical

### Integration style

- Separate internal domain events from public integration contracts
- Keep event handlers focused
- Use background processing where temporal decoupling matters

## Anti-Patterns to Avoid

- Shared database access across modules
- Shared mega-project for all DTOs
- Frontend referencing backend projects directly
- Business logic in FastEndpoints endpoint classes
- A massive shared kernel containing unrelated helpers
- Modules registering themselves through scattered host code instead of a single extension method
- Blindly sharing EF entities over HTTP
- Treating Kiota output as hand-maintained application code

## Example Target Startup Flow

At startup, the API host should:

1. Configure logging and defaults
2. Configure FastEndpoints and auth
3. Configure Swagger/OpenAPI
4. Register each module through one extension method
5. Register mediator and pipeline behaviors
6. Start the app and expose the OpenAPI document

At startup, the Blazor host should:

1. Register auth/session services
2. Register the Kiota client and HTTP pipeline
3. Register feature services
4. Route users through UI flows that call the backend SDK

## Deliverable Checklist for a Fresh App

The scaffold is complete only when all of the following exist:

- Solution and projects created
- Modular monolith dependency rules established
- FastEndpoints backend host running
- Blazor frontend running
- OpenAPI document available
- Kiota SDK generated
- Blazor calling the API through Kiota
- At least one end-to-end feature implemented across UI, API, use case, domain, and persistence
- Architecture tests enforcing boundary rules

## Summary

RiverBooks succeeds because it is opinionated about boundaries.

Copy these core ideas:

- Organize by bounded context
- Keep one host/composition root
- Use contracts for cross-module collaboration
- Keep persistence owned by the module
- Put orchestration in use cases
- Keep endpoints thin
- Keep the shared kernel small
- Enforce the architecture with tests

For future applications, add this additional rule:

- The frontend boundary is HTTP plus a Kiota-generated SDK, not direct project references.

That combination preserves RiverBooks' modular monolith strengths while making the UI stack cleaner, more replaceable, and more explicit.
