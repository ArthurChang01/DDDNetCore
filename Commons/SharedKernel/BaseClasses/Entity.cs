using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.BaseClasses
{
    public abstract class Entity<TId>
    {
        private int? _requestHashCode;
        private TId _Id;
        private readonly Queue<IEvent> _eventQueue;

        public Entity()
        {
            this._eventQueue = new Queue<IEvent>();
        }

        public virtual TId Id { get => this._Id; set => this._Id = value; }

        public void PushEvent(IEvent situation)
        {
            this._eventQueue.Enqueue(situation);
        }

        public IEnumerable<IEvent> IterateEvent()
        {
            IEvent current;
            while (this._eventQueue.TryDequeue(out current))
            {
                yield return current;
            }
        }

        public bool IsTransient()
        {
            return this.Id.Equals(default(TId));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TId>))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity<TId> item = obj as Entity<TId>;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!this._requestHashCode.HasValue)
                    this._requestHashCode = this.Id.GetHashCode() ^ 31;
                return this._requestHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !(left == right);
        }
    }
}