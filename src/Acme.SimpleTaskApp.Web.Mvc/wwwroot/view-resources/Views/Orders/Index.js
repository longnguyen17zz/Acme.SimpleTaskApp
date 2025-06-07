(function ($) {
	var _orderService = abp.services.app.order,
		l = abp.localization.getSource('SimpleTaskApp'),
		_$table = $('#OrdersTable');
	console.log(_orderService);

	var _$ordersTable = _$table.DataTable({
		paging: true,
		serverSide: true,
		processing: true,
		//pageLength: 5,
		//lengthMenu: [5, 10, 20, 50, 100],
		listAction: {
			ajaxFunction: _orderService.getPaged,
			inputFilter: function () {
				var filter = $('#OrderSearchForm').serializeFormToObject(true);
				console.log(filter);
				return filter;
			}
		},
		buttons: [
			{
				name: 'refresh',
				text: '<i class="fas fa-redo-alt"></i>',
				action: () => _$ordersTable.draw(false)
			}
		],
		responsive: {
			details: {
				type: 'column'
			}
		},
		columnDefs: [
			{
				targets: 0,
				className: 'control',
				defaultContent: '',
				orderable: false,
			},
			{
				targets: 1,
				data: 'fullName',
				className: 'text-center',
			},
			{
				targets: 2,
				data: 'phoneNumber',
				orderable: false,
				className: 'text-center',
			},
			{
				targets: 3,
				data: 'address',
				className: 'text-center',
			},
			{
				targets: 4,
				data: 'totalAmount',
				className: 'text-center',
				render: function (data, type, row, meta) {
					if (data) {
						var lang = abp.localization.currentLanguage.name;
						const VND_TO_USD_RATE = 25849;
						const usd = data / VND_TO_USD_RATE;

						var display = '';
						if (lang == "vi") {
							display = data.toLocaleString('vi-VN') + ' VND';
						} else if (lang == "en") {
							display = new Intl.NumberFormat('en-US', {
								style: 'currency',
								currency: 'USD',
								minimumFractionDigits: 3,
								maximumFractionDigits: 3
							}).format(usd);
						}

						return `<a href="javascript:void(0)" class="show-order-details" data-order-id="${row.id}">${display}</a>`;
					} else {
						return '<span class="text-muted">No price</span>';
					}
				}
			},
			{
				targets: 5,
				data: 'paymentMethod',
				className: 'text-center',

			},
			{
				targets: 6,
				data: 'status',
				className: 'text-center',
			},
			{
				targets: 7,
				data: 'orderDate',
				className: 'text-center',
				render: function (data, type, row) {
					if (!data) return '';
					const date = new Date(data);
					return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')
						}/${date.getFullYear()} - ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
				}
			},
			{
				targets: 8,
				data: null,
				orderable: false,
				autoWidth: false,
				defaultContent: '',
				className: 'text-center',
				render: function (data, type, row, meta) {
					if (row.status === "Đang xử lý") {
						return '<button class="btn btn-primary btn-sm">Confirm</button>';
					} else {
						return '<span class="text-success">Done</span>';
					}
				}

			}

		]
	});

	console.log(_$ordersTable);
	console.log(_$ordersTable.context);
	console.log(_$ordersTable.selector);

	$(document).on('click', '.delete-product', function () {
		var productId = $(this).attr("data-product-id");
		var productName = $(this).attr('data-product-name');

		deleteProduct(productId, productName);
	});

	function deleteProduct(productId, productName) {
		abp.message.confirm(
			abp.utils.formatString(
				l('AreYouSureWantToDelete'),
				productName),
			null,
			(isConfirmed) => {
				if (isConfirmed) {
					_productService.delete({
						id: productId
					}).done(() => {
						abp.notify.info(l('SuccessfullyDeleted'));
						_$productsTable.ajax.reload();
					}).fail((xhr) => {
						// chưa nhảy vào case check phân quyền 
						if (xhr.status === 403) {
							abp.message.warn("Bạn không có quyền xóa sản phẩm", "Thông báo")
						} else {
							abp.message.error('Có lỗi xảy ra khi xóa sản phẩm.', 'Lỗi');
						}
						abp.message.error('Có lỗi xảy ra khi xóa sản phẩm.', 'Lỗi');
					});
				}
			}
		);
	}

	$(document).on('click', ".show-order-details", function () {
		var orderId = $(this).data('order-id');
		console.log(orderId);
		// Gọi Ajax hoặc AppService để lấy chi tiết đơn hàng
		abp.ajax({
			url: abp.appPath + 'api/services/app/Order/GetOrderDetails?orderId=' + orderId,
			type: 'GET'
		}).done(function (result) {
			console.log("Tốt");
			var html = '';
			result.items.forEach(function (item) {
				html += `
                <tr>
                    <td>${item.productName}</td>
                    <td>${item.quantity}</td>
                    <td>${item.price.toLocaleString('vi-VN')} VND</td>
                </tr>`;
			});

			$('#orderDetailsTable tbody').html(html);
			$('#orderDetailsModal').modal('show');
		});
	})

	$('.btn-search,#FilterByDate').on('click', (e) => {
		e.preventDefault();
		_$ordersTable.ajax.reload();
	});

	$('.txt-search').on('keypress', (e) => {
		if (e.which == 13) {
			_$ordersTable.ajax.reload();
			return false;
		}
	});

	$(".filter-status").on('change', function (e) {
		e.preventDefault();
		_$ordersTable.ajax.reload();
	})


})(jQuery);
