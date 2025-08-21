using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Stocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acme.SimpleTaskApp.Entities.Batches
{
	[Table("AppBatches")]
	public class Batch : FullAuditedEntity<int>
	{
		public DateTime DateEntry { get; set; }
		public string Importer { get; set; }

		public ICollection<Stock> Stocks { get; set; }

		//public Batch(DateTime dateEntry, string importer)
		//{
		//	DateEntry = dateEntry;
		//	Importer = importer;
		//}
	}
}
