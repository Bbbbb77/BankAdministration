using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Persistence.Models;

namespace BankAdministration.Persistence.DTOS
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public static explicit operator User(UserDto dto) => new User
        {
            Id = dto.Id,
            UserName = dto.UserName,
            FullName = dto.FullName
        };

        public static explicit operator UserDto(User u) => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            FullName = u.FullName
        };
    }
}
