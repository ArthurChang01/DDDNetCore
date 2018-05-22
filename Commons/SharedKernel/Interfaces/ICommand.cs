using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Interfaces
{
    public interface ICommand<Tresp> : IRequest<Tresp>
    {
    }
}