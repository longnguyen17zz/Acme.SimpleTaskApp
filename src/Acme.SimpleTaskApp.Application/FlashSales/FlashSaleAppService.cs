using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Acme.SimpleTaskApp.Entities.FlashSaleItems;
using Acme.SimpleTaskApp.Entities.FlashSales;
using Acme.SimpleTaskApp.FlashSales.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading.Tasks;
using Abp.Timing;
using Abp.UI;
using Abp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace Acme.SimpleTaskApp.FlashSales
{
	public class FlashSaleAppService : SimpleTaskAppAppServiceBase, IFlashSaleAppService
	{
		private readonly IRepository<FlashSale> _flashSaleRepository;
		private readonly IRepository<FlashSaleItem> _flashSaleItemRepository;


		public FlashSaleAppService(IRepository<FlashSale> flashSaleRepository, IRepository<FlashSaleItem> flashSaleItemRepository)
		{
			_flashSaleRepository = flashSaleRepository;
			_flashSaleItemRepository = flashSaleItemRepository;
		}
		public async Task<List<FlashSaleDto>> GetAllAsync()
		{
			var flashSales = await _flashSaleRepository.GetAllIncluding(fs => fs.Items).ToListAsync();

			var result = flashSales.Select(fs => new FlashSaleDto
			{
				Id = fs.Id,
				Name = fs.Title,
				StartTime = fs.StartTime,
				EndTime = fs.EndTime,
				IsActive = fs.IsActive,
				Items = fs.Items.Select(item => new FlashSaleItemDto
				{
					Id = item.Id,
					ProductId = item.ProductId,
					ProductName = item.Product.Name,
					FlashSaleId = item.FlashSaleId,
					Sold = item.Sold,
					OriginPrice = item.Sold,
					SalePrice = item.FlashPrice,
					QuantityLimit = item.Quantity
				}).ToList()
			}).ToList();

			return result;
		}

		public async Task<FlashSaleDto> GetAsync(int id)
		{
			var fs = await _flashSaleRepository
					.FirstOrDefaultAsync(x => x.Id == id);

			return new FlashSaleDto
			{
				Id = fs.Id,
				Name = fs.Title,
				StartTime = fs.StartTime,
				EndTime = fs.EndTime,
				IsActive = fs.IsActive

			};
		}

		public async Task CreateAsync(FlashSaleDto input)
		{
			var entity = new FlashSale
			{
				Title = input.Name,
				StartTime = input.StartTime,
				EndTime = input.EndTime,
				IsActive = true,
			};

			await _flashSaleRepository.InsertAsync(entity);
		}

		[HttpPost]
		public async Task UpdateAsync(FlashSaleDto input)
		{
			var existing = await _flashSaleRepository
					.FirstOrDefaultAsync(x => x.Id == input.Id);

			if (existing == null)
			{
				throw new UserFriendlyException("Không tìm thấy FlashSale cần cập nhật");
			}

			existing.Title = input.Name;
			existing.StartTime = input.StartTime;
			existing.EndTime = input.EndTime;
			if(input.EndTime < Clock.Now)
			{
				if(input.IsActive == true)
				{
					throw new UserFriendlyException("Vui lòng cập nhật thời gian");
				}
				existing.IsActive = input.IsActive;
			}
			existing.IsActive = input.IsActive;
			await _flashSaleRepository.UpdateAsync(existing);
			CurrentUnitOfWork.SaveChanges();
		}



		public async Task DeleteAsync(int id)
		{
			await _flashSaleRepository.DeleteAsync(id);
		}

		public async Task<PagedResultDto<FlashSaleDto>> GetAllPaged(GetAllFlashSaleInput input)
		{
			var query = _flashSaleRepository
					.GetAllIncluding(x => x.Items)
					.AsQueryable()
					.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Keyword));

			var totalCount = await query.CountAsync();
			var items = await query
					.OrderByDescending(x => x.CreationTime)
					.PageBy(input)
					.ToListAsync();

			var result = items.Select(x => new FlashSaleDto
			{
				Id = x.Id,
				Name = x.Title,
				StartTime = x.StartTime,
				EndTime = x.EndTime,
				CreationTime = x.CreationTime,
				IsActive = x.IsActive,
				//IsActive = x.IsActive && x.EndTime > Clock.Now,


			}).ToList();

			return new PagedResultDto<FlashSaleDto>(totalCount, result);
		}


		public async Task ApplyForAsync(ApplyFlashSaleDto input)
		{
			var exists = await _flashSaleItemRepository.FirstOrDefaultAsync(item =>
					item.FlashSaleId == input.FlashSaleId &&
					item.ProductId == input.ProductId);

			if (exists != null)
			{
				throw new UserFriendlyException("Sản phẩm này đã được áp dụng cho Flash Sale.");
			}

			var flashSaleItem = new FlashSaleItem
			{
				FlashSaleId = input.FlashSaleId,
				ProductId = input.ProductId,
				FlashPrice = input.FlashPrice,
				Quantity = input.Quantity,
				CreationTime = Clock.Now
			};

			await _flashSaleItemRepository.InsertAsync(flashSaleItem);
		}

		public async Task<List<FlashSaleItemDto>> GetFlashSaleItemsByFlashSaleId(int flashSaleId)
		{
			var items = await _flashSaleItemRepository.GetAll().Include(fsi => fsi.Product)
						 .Where(i => i.FlashSaleId == flashSaleId)
						 .ToListAsync();
			var result = items.Select(i => new FlashSaleItemDto
			{
				Id = i.Id,
				FlashSaleId = i.FlashSaleId,
				ProductId = i.ProductId,
				ProductName = i.Product.Name,
				OriginPrice = i.Product.Price,
				SalePrice = i.FlashPrice,
				Sold = i.Sold,
				QuantityLimit = i.Quantity,

			}).ToList();

			return result;
		}

		public async Task<List<FlashSaleDto>> GetAllIsActiveAsync()
		{
			var flashSales = await _flashSaleRepository
			.GetAll().Include(fs => fs.Items)             // Bao gồm Items
			.ThenInclude(i => i.Product)                 // Bao gồm Product của mỗi Item
			.Where(fs => fs.IsActive)
			.ToListAsync();

			var result = flashSales.Select(fs => new FlashSaleDto
			{
				Id = fs.Id,
				Name = fs.Title,
				StartTime = fs.StartTime,
				EndTime = fs.EndTime,
				IsActive = fs.IsActive,
				Items = fs.Items.Select(item => new FlashSaleItemDto
				{
					Id = item.Id,
					ProductId = item.ProductId,
					ProductName = item.Product != null ? item.Product.Name : "N/A",
					FlashSaleId = item.FlashSaleId,
					Sold = item.Sold,
					OriginPrice = item.Sold,
					SalePrice = item.FlashPrice,
					QuantityLimit = item.Quantity
				}).ToList()
			}).ToList();

			return result;
		}


	}
}
