# netcore-repo-pattern
Repository pattern for dotnet core 2.2
## Basic information

### Items
* Generic repository
* Basic DbContext

### Usage

#### Step 1: Add Nuget package
* Go to manage nuget package
* Add Microsoft.EntityFrameworkCore to your project
* Or follow https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/

#### Step 2: Replace BasicDbContext with your own DbContext
```C#
public class YourDbContext : DbContext
{
}
```

#### Step 3: Make an entity
Ex:
```C#
public class Product{
    public int Id{ get; set; }
    public string Name{ get; set; }
}
```

#### Step 4: Create repository for the model above
Interface:
```C#
public interface IProductRepository : IGenericRepository<Product>
{
    // Your custom methods
}
```

Implementation
```C#
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly YourDbContext _context;
    public ProductRepository(YourDbContext context) : base(context)
    {
        _context = context;
    }
    // Implement your custom methods
}
```

#### Step 5(For Web): Register the repositories in Startup.cs file
```C#
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddScoped(typeof(IProductRepository<>), typeof(ProductRepository<>));
```

#### An additional step: If you have many repos to register, research this
https://www.thereformedprogrammer.net/asp-net-core-fast-and-automatic-dependency-injection-setup/

## Good luck and have a nice day!

