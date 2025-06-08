using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Acme.SimpleTaskApp.Entities.FlashSales;
using System;
using System.Linq;
using Abp.Domain.Uow;

namespace Acme.SimpleTaskApp.FlashSales
{
	public class FlashSaleStatusWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
	{
		private readonly IRepository<FlashSale> _flashSaleRepository;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public FlashSaleStatusWorker(
				AbpTimer timer,
				IRepository<FlashSale> flashSaleRepository,
				IUnitOfWorkManager unitOfWorkManager
		) : base(timer)
		{
			_flashSaleRepository = flashSaleRepository;
			_unitOfWorkManager = unitOfWorkManager;

			Timer.Period = 60 * 1000; // chạy mỗi phút
		}

		protected override void DoWork()
		{
			Logger.Info("Đang kiểm tra và cập nhật trạng thái FlashSale...");

			using (var uow = _unitOfWorkManager.Begin())
			{
				try
				{
					var now = Clock.Now;

					Logger.Info($"Thời gian hiện tại: {now}");

					if (_flashSaleRepository == null)
					{
						Logger.Error("_flashSaleRepository is null!");
						return;
					}

					var toActivate = _flashSaleRepository.GetAllList(x =>
							!x.IsActive && x.StartTime <= now && x.EndTime > now);

					foreach (var flashSale in toActivate)
					{
						Logger.Info($"Đang kích hoạt: {flashSale.Id}");
						flashSale.IsActive = true;
					}

					var toDeactivate = _flashSaleRepository.GetAllList(x =>
							x.IsActive && x.EndTime <= now);

					foreach (var flashSale in toDeactivate)
					{
						Logger.Info($"Đang hủy kích hoạt: {flashSale.Id}");
						flashSale.IsActive = false;
					}

					uow.Complete();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex.ToString(), ex);
				}
			}
		}

	}
}
