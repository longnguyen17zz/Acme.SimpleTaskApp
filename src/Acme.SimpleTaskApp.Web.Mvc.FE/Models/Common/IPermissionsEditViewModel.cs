using Acme.SimpleTaskApp.Roles.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Common;

public interface IPermissionsEditViewModel
{
    List<FlatPermissionDto> Permissions { get; set; }
}