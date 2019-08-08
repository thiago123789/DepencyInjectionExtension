using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using DependencyInjection.Extensions.Attributes;

namespace DependencyInjection.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection InjectAllServices(this IServiceCollection services)
        {
            var allAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                allAssemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(dll));
            }

            services
                .InjectDependencyByLifeCycle<TransientDependency>(
                    allAssemblies, 
                    ServiceLifetime.Transient)
                .InjectDependencyByLifeCycle<SingletonDependency>(
                    allAssemblies, 
                    ServiceLifetime.Singleton)
                .InjectDependencyByLifeCycle<ScopedDependency>(
                    allAssemblies, 
                    ServiceLifetime.Scoped);
            
            return services;
        }

        private static IServiceCollection InjectDependencyByLifeCycle<T>(this IServiceCollection services, IList<Assembly> assemblyList, ServiceLifetime lifeCycle) 
            where T : Attribute
        {
            var servicesInstances = assemblyList
                .SelectMany(x => x.GetTypes())
                .Where(e => e.CustomAttributes
                    .Any(k => k.AttributeType.Name == typeof(T).Name))
                .ToList();

            if (servicesInstances.Any())
            {
                foreach (var instance in servicesInstances)
                {
                    var attribute = instance.CustomAttributes.FirstOrDefault(x =>
                                x.AttributeType.Name == typeof(T).Name);
                    if (attribute != null && attribute.AttributeType.IsAssignableFrom(typeof(TransientDependency)))
                    {
                        var contractName = attribute.ConstructorArguments.FirstOrDefault(e => e.ArgumentType == typeof(Type));
                        if (contractName != null)
                        {
                            var interfaceType = instance.GetInterface(((Type)contractName.Value).Name);
                            if (interfaceType != null)
                            {
                                var descriptor = new ServiceDescriptor(interfaceType, instance, lifeCycle);
                                services.Add(descriptor);
                            }
                            else
                            {
                                throw new Exception($"[{typeof(T).Name.ToString()} attribute at {instance.Name} class]: Must receive a interface as a param ");
                            }                            
                        }
                    }                    
                }
            }

            return services;
        }

    }
}
