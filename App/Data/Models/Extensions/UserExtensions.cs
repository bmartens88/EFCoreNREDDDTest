using App.Dto;

namespace App.Data.Models.Extensions;

public static class UserExtensions
{
    public static UserDto AsDto(this User user)
    {
        return new(user.Id, user.FirstName, user.LastName, user.MiddleName);
    }
}
