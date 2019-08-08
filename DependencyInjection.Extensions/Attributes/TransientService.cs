using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Extensions.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false )]
    public class TransientDependency : Attribute
    {
        public Type Contract { get; private set; }
        public TransientDependency(Type contract)
        {
            this.Contract = contract;
        }
    }
}
