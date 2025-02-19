# Bodatero.Result

## Overview

**Bodatero.Result** is a domain package that provides a standardized way to handle operation results across an application. It encapsulates success and failure states, eliminating the need for throwing exceptions in expected scenarios such as validation errors.

This package serves as the foundation for other sub-packages, including `Bodatero.Result.AspNetCore`, which extends its capabilities for API responses.

---

## Why Use `Result<T>` Instead of Exceptions?

### ✅ Benefits Over Throwing Exceptions:

1. **Performance** - Exceptions are costly, as they involve stack unwinding and capturing detailed diagnostics.
2. **Predictability** - Results clearly indicate success or failure without relying on catching exceptions.
3. **Cleaner Code** - Avoids excessive try-catch blocks, making the business logic more readable.
4. **Encapsulation** - Error details are stored in a structured format rather than being scattered across exception handling.

---

## Usage



### Using `Result<T>` in Business Logic

```csharp
public class ProductService
{
    public Result<Product> CreateProduct(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Exception("Name must not be empty");
        if (string.IsNullOrWhiteSpace(description))
            return new Exception("Name must not be empty");

        var product = new Product { Name = name, Description = description };

        return product;
    }
}
```

This approach keeps the logic clear and prevents unnecessary exceptions from being thrown for validation failures.

---