using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Data.Models.ValueObjects;

public record struct UserId
{
    public Guid Value { get; private set; }

    public UserId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(UserId userId)
        => userId.Value;

    public static implicit operator UserId(Guid value)
        => new(value);

    public static UserId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public class StronglyTypedIdEfValueConverter : ValueConverter<UserId, Guid>
    {
        public StronglyTypedIdEfValueConverter(ConverterMappingHints? mappingHints = null)
            : base(id => id.Value, value => new UserId(value), mappingHints) { }
    }
}