namespace Ordering.Core.Entities.Base
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public abstract class EntityBase<TId> : IEntityBase<TId>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public virtual TId Id { get; protected set; }
        int? _requestedHashCode;
        public bool IsTransient() => Id.Equals(default(TId));

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityBase<TId>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (EntityBase<TId>)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item == this;

        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;
                return _requestedHashCode.Value;

            }

            return base.GetHashCode();
        }

        public static bool operator ==(EntityBase<TId> left,
                                       EntityBase<TId> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right) => !(left == right);
    }
}