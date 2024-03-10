using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehiGate.Application.Common.Models;

namespace VehiGate.Application.Users.Queries.GetUsersList;
public class UsersListDto
{
    public required List<UserModel> Data { get; set; }

    public int Total { get; set; }
}

