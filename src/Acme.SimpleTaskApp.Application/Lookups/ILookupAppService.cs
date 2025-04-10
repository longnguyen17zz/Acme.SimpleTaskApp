using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Lookups;
public interface ILookupAppService : IApplicationService
{
    Task<ListResultDto<ComboboxItemDto>> GetPeopleComboboxItems();
}
