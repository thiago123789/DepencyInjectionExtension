using System;

namespace DependencyInjection.Extensions.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    class SingletonDependency : Attribute
    {
        public Type Contract { get; private set; }
        public SingletonDependency(Type contract)
        {
           this.Contract = contract;
        }
    }
}