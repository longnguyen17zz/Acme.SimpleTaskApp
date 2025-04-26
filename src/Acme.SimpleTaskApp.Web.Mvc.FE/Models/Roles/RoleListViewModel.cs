using Acme.SimpleTaskApp.Roles.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Roles;

public class RoleListViewModel
{
    public IReadOnlyList<PermissionDto> Permissions { get; set; }
}
