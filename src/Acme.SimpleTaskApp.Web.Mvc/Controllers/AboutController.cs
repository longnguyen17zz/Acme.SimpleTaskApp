using Abp.AspNetCore.Mvc.Authorization;
using Acme.SimpleTaskApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Acme.SimpleTaskApp.Web.Controllers;

[AbpMvcAuthorize]
public class AboutController : SimpleTaskAppControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
