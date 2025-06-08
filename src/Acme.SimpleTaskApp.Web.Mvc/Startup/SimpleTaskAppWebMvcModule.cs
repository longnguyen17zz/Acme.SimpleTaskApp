using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Acme.SimpleTaskApp.Configuration;
using Acme.SimpleTaskApp.FlashSales;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Acme.SimpleTaskApp.Web.Startup;

[DependsOn(typeof(SimpleTaskAppWebCoreModule))]
public class SimpleTaskAppWebMvcModule : AbpModule
{
	private readonly IWebHostEnvironment _env;
	private readonly IConfigurationRoot _appConfiguration;

	public SimpleTaskAppWebMvcModule(IWebHostEnvironment env)
	{
		_env = env;
		_appConfiguration = env.GetAppConfiguration();
	}

	public override void PreInitialize()
	{
		Configuration.Navigation.Providers.Add<SimpleTaskAppNavigationProvider>();
	}

	public override void Initialize()
	{
		IocManager.RegisterAssemblyByConvention(typeof(SimpleTaskAppWebMvcModule).GetAssembly());
	}

	public override void PostInitialize()
	{
		var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
		workManager.Add(IocManager.Resolve<FlashSaleStatusWorker>());
	}
}
