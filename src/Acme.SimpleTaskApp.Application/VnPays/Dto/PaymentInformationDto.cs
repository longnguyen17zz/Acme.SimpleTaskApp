﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.VnPays.Dto
{
	public class PaymentInformationDto
	{
		public string OrderType { get; set; }
		public double Amount { get; set; }
		public string OrderDescription { get; set; }
		public string Name { get; set; }
	}
}
