using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Users
{
    public class UserApiModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public static UserApiModel FromUser(User user)
            => new UserApiModel
            {
                UserId = user.UserId,
                Username = user.Username
            };
    }
}
