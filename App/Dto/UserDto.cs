using App.Data.Models;

namespace App.Dto;

public record struct UserDto(Guid Id, string FirstName, string LastName, string? MiddleName = null)
{
    public User AsUser()
    {
        return User.CreateNew(FirstName, LastName, MiddleName);
    }
}