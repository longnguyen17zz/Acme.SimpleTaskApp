using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Acme.SimpleTaskApp.Batchs.Dto;
using Acme.SimpleTaskApp.Entities.Batches;
using Acme.SimpleTaskApp.Stocks.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;



namespace Acme.SimpleTaskApp.Batchs
{
	public class BatchAppService : SimpleTaskAppAppServiceBase, IBatchAppService
	{
		private readonly IRepository<Batch> _batchRepository;

		public BatchAppService(IRepository<Batch> batchRepository)
		{
			_batchRepository = batchRepository;
		}
		[HttpPost]
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

		public async Task<PagedResultDto<BatchDto>> GetPagedAsync(BatchInput input)
		{

			var query = _batchRepository
					.GetAll()
					.Include(p => p.Stocks);
			
			//if (!string.IsNullOrWhiteSpace(input.Keyword))
			//{
			//	query = query.Where(p => p.Product.Name.ToLower().Contains(input.Keyword.ToLower()));
			//}

			//if (input.FromDate.HasValue)
			//{
			//	query = query.Where(x => x.CreationTime >= input.FromDate.Value);
			//}

			//if (input.ToDate.HasValue)
			//{
			//	query = query.Where(x => x.CreationTime <= input.ToDate.Value.AddDays(1));
			//}

			//if(input.BatchId != null)
			//{
			//	query = query.Where(x => x.BatchId == input.BatchId);
			//}

			var totalCount = await query.CountAsync();
			var batches = await query
					.OrderBy(input.Sorting)
					.PageBy(input)
					.ToListAsync();

			var result = batches.Select(p => new BatchDto
			{
				Id = p.Id,
				DateEntry = p.DateEntry,
				Importer = p.Importer,
				//Stocks = p.Stocks.Select(s => new StockDto
				//{
				//	Id = s.Id,
				//	ProductId = s.ProductId,
				//	Quantity = s.Quantity,
				//	CreationTime = s.CreationTime,
				//	ProductName = s.Product.Name,
				//	ProductCategoryName = s.Product.Category != null ? s.Product.Category.Name : "Không có danh mục"
				//}).ToList()

			}).ToList();
			return new PagedResultDto<BatchDto>(totalCount, result);
		}
	}
}
