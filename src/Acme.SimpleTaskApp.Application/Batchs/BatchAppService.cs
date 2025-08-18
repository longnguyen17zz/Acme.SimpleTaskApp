using Abp.Domain.Repositories;
using Acme.SimpleTaskApp.Batchs.Dto;
using Acme.SimpleTaskApp.Entities.Batches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Batchs
{
	public class BatchAppService : SimpleTaskAppAppServiceBase, IBatchAppService
	{
		private readonly IRepository<Batch> _batchRepository;

		public BatchAppService(IRepository<Batch> batchRepository)
		{
			_batchRepository = batchRepository;
		}

		public async Task CreateAsync(CreateBatchInputDto input)
		{
			if (input == null)
			{
				throw new ArgumentNullException(nameof(input), "Batch input cannot be null");
			}

			var batch = new Batch
			{
				DateEntry = DateTime.Now,
				Importer = input.Importer
			};
			await _batchRepository.InsertAsync(batch);
		}
	}
}
