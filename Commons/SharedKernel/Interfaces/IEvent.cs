using MediatR;

namespace SharedKernel.Interfaces
{
    public interface IEvent : INotification
    {
        string Id { get; }
        int VersionNo { get; }
    }
}