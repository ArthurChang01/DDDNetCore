using MediatR;

namespace SharedKernel.Interfaces
{
    public interface ICommandHandler<Tcmd, Trtn> : IRequestHandler<Tcmd, Trtn>
        where Tcmd : IRequest<Trtn>
    {
    }
}