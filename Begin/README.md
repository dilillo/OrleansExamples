# About this code
This code is a simple example of how .NET Orleans can be leveraged in an API setting to bring substantial benefits to a line of business application.  Specifically, it can show how easy it is to get running and how it schedules calls to hosted grains.

# Steps

## 1 - Add a nuget reference to Microsoft.Orleans.Server
``` xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="7.0.8" />
    <PackageReference Include="Microsoft.Orleans.Server" Version="7.1.2" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
  </ItemGroup>

</Project>
```

## 2 - Create the IProductGrain interface
``` csharp
public interface IProductGrain : IGrainWithStringKey
{
    Task<ProductGrainState> GetState();

    Task Register(string registerTo);
}
```

## 3 - Create the ProductGrainState class
``` csharp
[GenerateSerializer]
public record class ProductGrainState
{
    [Id(0)]
    public DateTime? RegisteredOn { get; set; }

    [Id(1)]
    public string? RegisteredTo { get; set; }
}
```

## 4 - Create the ProductGrainState class
``` csharp
public class ProductGrain : Grain, IProductGrain
{
    private readonly ILogger<ProductGrain> _logger;
    private readonly IPersistentState<ProductGrainState> _state;

    public ProductGrain(
        ILogger<ProductGrain> logger,
        [PersistentState(stateName: "product", storageName: "products")] IPersistentState<ProductGrainState> state)
    {
        _logger = logger;
        _state = state;
    }

    public Task<ProductGrainState> GetState() => Task.FromResult(_state.State);

    public async Task Register(string registerTo)
    {
    }
}
```

## 5 - Implement the Register method
``` csharp
public class ProductGrain : Grain, IProductGrain
{
    private readonly ILogger<ProductGrain> _logger;
    private readonly IPersistentState<ProductGrainState> _state;

    public ProductGrain(
        ILogger<ProductGrain> logger,
        [PersistentState(stateName: "product", storageName: "products")] IPersistentState<ProductGrainState> state)
    {
        _logger = logger;
        _state = state;
    }

    public Task<ProductGrainState> GetState() => Task.FromResult(_state.State);

    public async Task Register(string registerTo)
    {
        var canRegister = await ComplexTimeConsumingOrUnreliableBusinessLogic();

        if (canRegister)
        {
            _state.State = new()
            {
                RegisteredOn = DateTime.Now,
                RegisteredTo = registerTo
            };

            await _state.WriteStateAsync();

            _logger.LogInformation("Registered product {SerialNumber} to {RegisteredTo}", this.GetPrimaryKeyString(), _state.State.RegisteredTo);
        }
    }

    private const int ComplexTimeConsumingOrUnreliableBusinessLogicDelay = 0;

    private async Task<bool> ComplexTimeConsumingOrUnreliableBusinessLogic()
    {
        await Task.Delay(ComplexTimeConsumingOrUnreliableBusinessLogicDelay);

        return _state.State?.RegisteredOn == null;
    }
}
```

## 6 - Replace the ProductOperations implementation with one that leverages the grain
``` csharp
public class ProductOperations : IProductOperations
{
    private readonly IGrainFactory _grains;

    public ProductOperations(IGrainFactory grains)
    {
        _grains = grains;
    }

    public async Task<ProductDetailsModel> GetDetails(string serialNumber)
    {
        var grain = _grains.GetGrain<IProductGrain>(serialNumber);

        var grainState = await grain.GetState();

        return new ProductDetailsModel
        {
            SerialNumber = grain.GetPrimaryKeyString(),
            RegisteredOn = grainState.RegisteredOn,
            RegisteredTo = grainState.RegisteredTo
        };
    }

    public async Task Register(string serialNumber, string registerTo)
    {
        var grain = _grains.GetGrain<IProductGrain>(serialNumber);

        await grain.Register(registerTo);
    }
}
```

## 7 - Register Orleans in Program.cs
``` csharp
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("products");
});
```

## 8 - Bonus: Increase the delay in the ProductGrain to 10 seconds and observe behavior
``` csharp
    private const int ComplexTimeConsumingOrUnreliableBusinessLogicDelay = 10000;
```
