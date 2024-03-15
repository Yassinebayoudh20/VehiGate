using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehiGate.Application.Common.Models;

namespace VehiGate.Application.Users.Queries.GetUserInfo;

public class UserInfoDto
{
    public required string Id { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public List<string> Roles { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserModel, UserInfoDto>();
        }
    }
}
