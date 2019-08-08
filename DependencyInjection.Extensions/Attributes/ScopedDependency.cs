using System;

namespace DependencyInjection.Extensions.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    public class ScopedDependency : Attribute
    {
        public Type Contract { get; private set; }
        public ScopedDependency(Type contract)
        {
           this.Contract = contract;
        }
    }
}