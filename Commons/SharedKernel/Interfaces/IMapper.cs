using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Interfaces
{
    public interface IMapper<Tout, Tin>
    {
        Tin MapIn(Tout input);

        Tout MapOut(Tin output);
    }
}