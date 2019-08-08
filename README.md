# Depency Injection Extension
Lightweight Library in .NET Standard for Dependency Injection

Nuget: https://www.nuget.org/packages/DependencyInjection.Extensions/

## How to inject:

#### Use one of attributes bellow to mark any service or repository:
- TransientDependency
- ScopedDependency
- SingletonDependency

Example:
```csharp
[TransientDependency(typeof(IService))]
public class Service : IService
{
    public Service()
    {

    }
}

```

Format: ```[<AttribuleLyfeCycle>(typeof(<InterfaceType>))]```

#### In the Startup class call the ```InjectAllServices``` extension method as show the code snippet bellow:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    services.InjectAllServices();
}
```

## Tested with:

- [ ] .NET Core 1.1
- [ ] .NET Core 2.1
- [x] .NET Core 2.2
- [ ] .NET Core 3.0
