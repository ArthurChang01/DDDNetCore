using SharedKernel.Interfaces;
using System;

namespace SharedKernel.BaseClasses
{
    public class DomainEvent<T> : IEvent
    {
        public DomainEvent(int versionNo, T root)
        {
            this.Id = Guid.NewGuid().ToString();
            this.VersionNo = versionNo;
            this.Root = root;
        }

        public T Root { get; }

        public string Id { get; }

        public int VersionNo { get; }
    }
}