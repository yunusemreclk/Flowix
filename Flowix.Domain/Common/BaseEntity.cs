namespace Flowix.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7();
}
