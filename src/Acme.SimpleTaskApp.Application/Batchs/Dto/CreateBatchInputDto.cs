using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Batchs.Dto
{
	public  class CreateBatchInputDto
	{
		public DateTime DateEntry { get; set; }
		public string Importer { get; set; }
	}
}
