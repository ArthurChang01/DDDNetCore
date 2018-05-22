using SharedKernel.BaseClasses;
using System;
using System.Collections.Generic;

namespace Orders.Models
{
    public class Coffee : ValueObject
    {
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}