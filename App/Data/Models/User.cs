using App.Data.Models.ValueObjects;

namespace App.Data.Models;

public sealed class User : BaseEntity<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? MiddleName { get; private set; }


    public static User CreateNew(string firstName, string lastName, string? middleName = null)
    {
        return new(UserId.CreateUnique(), firstName, lastName, middleName);
    }

    private User(UserId id, string firstName, string lastName, string? middleName = null)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public void UpdateFirstName(string firstName)
    {
        FirstName = firstName;
    }
}
