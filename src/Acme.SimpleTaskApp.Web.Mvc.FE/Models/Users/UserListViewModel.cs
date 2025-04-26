using Acme.SimpleTaskApp.Roles.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
