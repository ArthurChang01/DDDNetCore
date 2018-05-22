using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedKernel.Interfaces
{
    public interface IConst
    {
        string Version { get; }

        string Name { get; }

        Assembly Assembly { get; }

        IDictionary<string, Type> Receiver { get; }
    }
}