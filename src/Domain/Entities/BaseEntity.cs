namespace Domain.Entities;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; protected internal set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        return Equals(obj as BaseEntity);
    }

    public bool Equals(BaseEntity? other)
    {
        if (other == null)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id);

    public static bool operator ==(BaseEntity? left, BaseEntity? right) => Equals(left, right);

    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !Equals(left, right);
}
