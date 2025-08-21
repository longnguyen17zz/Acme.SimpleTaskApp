using Abp.Application.Services;
using Acme.SimpleTaskApp.VnPays.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.VnPays
{
	public  interface IVnPayService : IApplicationService
	{
		string CreatePaymentUrl(PaymentInformationDto paymentInfo, HttpContext context);
		PaymentResponse PaymentExecute(IQueryCollection collections);
	}
}
