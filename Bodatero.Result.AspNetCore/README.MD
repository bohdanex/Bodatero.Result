# Bodatero.Result.AspNetCore

## Overview
Bodatero.Result.AspNetCore is an extension for ASP.NET Core Minimal APIs that simplifies handling endpoint responses using the `Result<T>` pattern. Instead of throwing exceptions for validation errors or failures, this approach ensures a structured and predictable API response handling mechanism.

## Why Use Result<T> Instead of Throwing Exceptions?

1. **Performance**: Throwing exceptions is costly in terms of performance, while returning `Result<T>` avoids unnecessary stack unwinding.
2. **Predictability**: API responses become more structured and avoid unexpected 500 errors.
3. **Customizability**: Developers can define custom success and failure handlers, improving flexibility in API design.
4. **Better Client Experience**: Clients receive consistent response structures, making error handling easier.

## Installation

To use `Bodatero.Result.AspNetCore`, install it via NuGet:

```sh
 dotnet add package Bodatero.Result.AspNetCore
```

## Usage

### 1. Register the Result Service

Modify `Program.cs` to register the `ResultService`:

```csharp
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddResult(cfg =>
{
    cfg.SuccessResultHandler = (context, value) => new OkResponse(value);
    cfg.FailResultHandler = (context, error) => new BadResponse(error);
});

var app = builder.Build();
```

### 2. Define Response Models

```csharp
public record class BadResponse(Exception Error, int StatusCode = 400);
public record class OkResponse(object? Data, int StatusCode = 200);
```

### 3. Using the Service in Minimal API Endpoints

#### Example 1: Using `WithResult<T>` for a Specific Endpoint

```csharp
var apiGroup = app.MapGroup("api/products");

apiGroup.MapPost("", ([FromBody] CreateProductRequest createProductRequest) =>
{
    Result<Product> CreateProduct()
    {
        List<string> validationErrors = new();

        if (string.IsNullOrEmpty(createProductRequest.Name))
        {
            validationErrors.Add("Name must not be empty");
        }

        if (string.IsNullOrEmpty(createProductRequest.Description))
        {
            validationErrors.Add("Description must not be empty");
        }

        return validationErrors.Any()
            ? Result<Product>.Fail(new ValidationException(string.Join('\n', validationErrors)))
            : Result<Product>.Success(new Product { Name = createProductRequest.Name, Description = createProductRequest.Description });
    }

    return CreateProduct();
}).WithResult<Product>();
```

#### Example 2: Apply `WithResult` to an Entire Route Group

```csharp
var apiGroup = app.MapGroup("api/products").WithResult(); // Applied to whole group

apiGroup.MapDelete("{id}", (Guid id) =>
{
    // return productService.Delete(id);
}); // `WithResult` handler will be executed
```

#### Example 3: Custom Result Handler for a Specific Endpoint

```csharp
apiGroup.MapDelete("{id}", (Guid id) =>
{
    // return productService.Delete(id);
})
.WithResult(new ResultServiceConfig(
    (context, value) => new OkResponse(value), // Custom success handler
    (context, error) => new BadResponse(error)  // Custom error handler
));
```