(function ($) {
    var _flashSaleService = abp.services.app.flashSale,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#FlashSaleCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#FlashSalesTable');
    console.log(_flashSaleService);
    var _$flashSaleTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        listAction: {
            ajaxFunction: _flashSaleService.getAllPaged,
            inputFilter: function () {
                var filter = $('#FlashSaleSearchForm').serializeFormToObject(true);
                console.log(filter);
                return filter;
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$flashSaleTable.draw(false)
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
                orderable: false
            },
            {
                targets: 1,
                data: 'name',
                className: 'text-center'
            },
            {
                targets: 2,
                data: 'startTime',
                className: 'text-center',
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                targets: 3,
                data: 'endTime',
                className: 'text-center',
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                targets: 4,
                data: 'isActive',
                className: 'text-center',
              render: function (data) {
                  console.log(data)
                    if (data == true) {
                        return '<span class="badge badge-success">Đang chạy</span>';
                    } else {
                        return '<span class="badge badge-secondary">Tạm dừng</span>';
                    }
                }
            },
            {
                targets: 5,
                data: null,
                orderable: false,
                className: 'text-center',
               
              render: (data, type, row, meta) => { // data: giá trị, type: kiểu xử lý , row là toàn bộ dữ liêu của hàng đó , meta là vị trị của ô đó  
                console.log(row);
                    return `<div class="dropdown">
                         <button class="btn btn-sm btn-primary dropdown-toggle" type="button" id="actionDropdown_${row.id}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              ${l('Actions')}
                         </button>
                         <div class="dropdown-menu p-0" aria-labelledby="actionDropdown_${row.id}">
                             <button type="button" class="dropdown-item text-secondary edit-flashSale" data-flashsale-id="${row.id}" data-toggle="modal" data-target="#FlashSaleEditModal">
                                 <i class="fas fa-edit mr-2"></i>  ${l('Edit')}
                             </button>
                             <div class="dropdown-divider m-0"></div>
                             <button type="button" class="dropdown-item text-secondary apply-flashSale" data-flashsale-id="${row.id}" data-flashsale-name="${row.name}">
                                 <i class="fas fa-eye mr-2"></i>  ${l('ApplyFor')}
                             </button>  <div class="dropdown-divider m-0"></div>
                             <button type="button" class="dropdown-item text-secondary view-flashSale" data-flashsale-id="${row.id}" data-flashsale-name="${row.name}">
                                 <i class="fas fa-eye mr-2"></i>  ${l('View')}
                             </button>
                             <div class="dropdown-divider m-0"></div>
                             <button type="button" class="dropdown-item text-danger delete-flashSale" data-flashsale-id="${row.id}" data-flashsale-name="${row.name}">
                                 <i class="fas fa-trash-alt mr-2"></i>  ${l('Delete')}
                             </button>
                         </div>
                     </div>`;

                }
            }
        ]
    });
  console.log(_$flashSaleTable);
  console.log(_$flashSaleTable.context);
  console.log(_$flashSaleTable.selector);
    // Date formatter helper
    function formatDate(dateStr) {
        if (!dateStr) return '';
        const date = new Date(dateStr);
        return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')
            }/${date.getFullYear()} ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    }

    // Form validation
    _$form.validate({
        rules: {
            Name: { required: true },
            StartTime: { required: true },
            EndTime: { required: true }
        }
    });

    // Save
    _$form.find('.save-button').on('click', function (e) {
        e.preventDefault();
        if (!_$form.valid()) return;

        var formData = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        $.ajax({
            url: abp.appPath + 'api/services/app/flashSale/create',
            type: 'POST',
            data: JSON.stringify(formData),
            contentType: 'application/json',
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success('Lưu thành công');
                _$flashSaleTable.ajax.reload();
            },
            error: function (xhr) {
                alert("Lỗi tạo flashSale: " + xhr.responseText);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal);
            }
        });
    
    });

    // Edit
    $(document).on('click', '.edit-flashSale', function () {
        var flashSaleId = $(this).data('flashsale-id');
        console.log(flashSaleId)
        abp.ajax({
            url: abp.appPath + 'FlashSales/EditModal?flashSaleId=' + flashSaleId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#FlashSaleEditModal .modal-content').html(content);
            }
        });
    });

    abp.event.on('flashsale.edited', function () {
        _$flashSaleTable.ajax.reload();
    });
    // Delete
    $(document).on('click', '.delete-flashSale', function () {
        var flashSaleId = $(this).data('flashsale-id');
        var flashSaleName = $(this).data('flashsale-name');
        deleteFlashSale(flashSaleId, flashSaleName);
    });
    function deleteFlashSale(flashSaleId, flashSaleName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                flashSaleName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _flashSaleService.delete(
                        flashSaleId
                    ).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$flashSaleTable.ajax.reload();
                    }).fail((xhr) => {
                        abp.message.error('Có lỗi xảy ra khi xóa sản phẩm.', 'Lỗi');
                    });
                }
            }
        );
    }
    // Modal apply
    // Apply FlashSale
    $(document).on('click', '.apply-flashSale', function () {
        var flashSaleId = $(this).data('flashsale-id');
        var flashSaleName = $(this).data('flashsale-name');
        // Gán thông tin vào modal
        console.log(flashSaleId);
        console.log(flashSaleName);

        $('#flashSaleId').val(flashSaleId);
        $('#flashSaleName').text(flashSaleName);

        console.log(abp.services.app.category.getAll().done());
        // Load danh sách Category
        abp.services.app.category.getAll().done(function (categories) {
            var $catSelect = $('#categorySelect');
            console.log(categories);
            $catSelect.empty();
            $.each(categories.items, function (i, item) {
                $catSelect.append(`<option value="${item.id}">${item.name}</option>`);
            });
        });
        $('#categorySelect').on('change', function () {
            var categoryId = $(this).val();
            console.log(categoryId)
            var $productSelect = $('#productSelect');
            $productSelect.empty();

            if (!categoryId) {
                $productSelect.append('<option value="">-- Chọn sản phẩm --</option>');
                return;
            }
            abp.services.app.product.getByCategoryId(categoryId).done(function (products) {
                console.log(products.items)
                $productSelect.append('<option value="">-- Chọn sản phẩm --</option>');
                $.each(products.items, function (i, item) {
                    $productSelect.append(`<option value="${item.id}">${item.name}</option>`);
                   
                });
            });

        });
        $('#productSelect').on('change', function () {
            var producId = $(this).val();
            abp.services.app.product.getByIdProduct(producId).done(function (product) {
                $("#priceProduct").val(product.price);
            });
        });
        // Hiện modal
        $('#applyForModal').modal('show');
    });
    // View Detail FlashSale
    $(document).on('click', '.view-flashSale', function () {
        var flashSaleId = $(this).data('flashsale-id');
        var flashSaleName = $(this).data('flashsale-name');

        $('#viewFlashSaleName').text(flashSaleName);

        // Gọi API lấy danh sách FlashSaleItem
        abp.services.app.flashSale.getFlashSaleItemsByFlashSaleId(flashSaleId).done(function (items) {
            var $tbody = $('#FlashSaleItemsTable tbody');
            $tbody.empty();
            console.log(items);
            if (items.length === 0) {
                $tbody.append('<tr><td colspan="4" class="text-center">Không có sản phẩm áp dụng</td></tr>');
            } else {
                $.each(items, function (i, item) {
                    $tbody.append(`
                    <tr>
                        <td>${item.productName}</td>
                        <td>${item.originPrice.toLocaleString()} đ</td>
                        <td>${item.salePrice.toLocaleString()} đ</td>
                        <td>${item.quantityLimit}</td>
                        <td>${item.sold}</td>
                    </tr>
                `);
                });
            }
            // Hiện modal
            $('#ViewFlashSaleItemsModal').modal('show');
        }).fail(function (err) {
            abp.notify.error('Lỗi khi tải chi tiết FlashSaleItem.');
        });
    });
})(jQuery);
