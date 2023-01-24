namespace App.Data.Models;

public abstract class BaseEntity<TId> where TId : notnull
{
    public TId Id { get; private set; }

    protected BaseEntity(TId id)
    {
        Id = id;
    }
}
