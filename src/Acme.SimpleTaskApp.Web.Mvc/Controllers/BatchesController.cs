﻿using Acme.SimpleTaskApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Acme.SimpleTaskApp.Web.Controllers
{
	public class BatchesController : SimpleTaskAppControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
