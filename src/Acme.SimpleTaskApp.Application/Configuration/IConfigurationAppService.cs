using Acme.SimpleTaskApp.Configuration.Dto;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
