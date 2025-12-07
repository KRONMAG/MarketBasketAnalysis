# <a id="MarketBasketAnalysis_ServiceCollectionExtensions"></a> Class ServiceCollectionExtensions

Namespace: [MarketBasketAnalysis](MarketBasketAnalysis.md)  
Assembly: MarketBasketAnalysis.dll  

Provides extension methods for registering Market Basket Analysis services in a dependency injection container.

```csharp
[PublicAPI]
public static class ServiceCollectionExtensions
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[ServiceCollectionExtensions](MarketBasketAnalysis.ServiceCollectionExtensions.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.MemberwiseClone\(\)](https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Methods

### <a id="MarketBasketAnalysis_ServiceCollectionExtensions_AddMarketBasketAnalysis_Microsoft_Extensions_DependencyInjection_IServiceCollection_"></a> AddMarketBasketAnalysis\(IServiceCollection\)

Registers all core Market Basket Analysis services in the provided <xref href="Microsoft.Extensions.DependencyInjection.IServiceCollection" data-throw-if-not-resolved="false"></xref>.

```csharp
public static IServiceCollection AddMarketBasketAnalysis(this IServiceCollection services)
```

#### Parameters

`services` [IServiceCollection](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection)

The service collection to add the Market Basket Analysis services to.

#### Returns

 [IServiceCollection](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection)

The same <xref href="Microsoft.Extensions.DependencyInjection.IServiceCollection" data-throw-if-not-resolved="false"></xref> instance so that additional calls can be chained.

#### Remarks

This method registers the following services as singletons:
<ul><li><xref href="MarketBasketAnalysis.Analysis.IMaximalCliqueAlgorithm" data-throw-if-not-resolved="false"></xref> (implemented by <xref href="MarketBasketAnalysis.Analysis.TomitaAlgorithm" data-throw-if-not-resolved="false"></xref>)</li><li><xref href="MarketBasketAnalysis.Analysis.IMaximalCliqueFinder" data-throw-if-not-resolved="false"></xref> (implemented by <xref href="MarketBasketAnalysis.Analysis.MaximalCliqueFinder" data-throw-if-not-resolved="false"></xref>)</li><li><xref href="MarketBasketAnalysis.Mining.IMinerFactory" data-throw-if-not-resolved="false"></xref> (implemented by <xref href="MarketBasketAnalysis.Mining.MinerFactory" data-throw-if-not-resolved="false"></xref>)</li></ul>

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">services</code> is <code>null</code>.

