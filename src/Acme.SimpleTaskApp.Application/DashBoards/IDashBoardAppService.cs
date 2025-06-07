using Abp.Application.Services;
using Acme.SimpleTaskApp.DashBoards.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.DashBoards
{
     public interface IDashBoardAppService : IApplicationService
    {

        public Task<DashBoardDto> GetDashboardDataAsync();
    }
}
