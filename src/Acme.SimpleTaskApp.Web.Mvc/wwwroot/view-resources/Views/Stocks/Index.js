(function ($) {
    var _stockService = abp.services.app.stock,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#StockCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#StocksTable');
    console.log(_stockService);

    var _$stocksTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        //pageLength: 5,
        //lengthMenu: [5, 10, 20, 50, 100],
        listAction: {
            ajaxFunction: _stockService.getPaged,
            inputFilter: function () {
                var filter = $('#ProductsSearchForm').serializeFormToObject(true);
                console.log(filter);
                //return $('#ProductsSearchForm').serializeFormToObject(true);
                return filter;
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$stocksTable.draw(false)
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
                data: 'productName',
                className: 'text-center',
                //render: function (data, type, row) {
                    
                //}
            },
            {
                targets: 2,
                data: 'categoryName',
                orderable: false,
                className: 'text-center',
                render: function (data, type, row) {
                    return data ? data : 'Không có danh mục';
                }
            },
            {
                targets: 3,
                data: 'stockQuantity',
                className: 'text-center',
            },
            {
                targets: 4,
                data: 'lastModificationTime',
                className: 'text-center',
                render: function (data, type, row) {
                    if (!data) return '';
                    const date = new Date(data);
                    return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')
                        }/${date.getFullYear()} - ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
                }
            },
            {
                targets: 5,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                className: 'text-center',
                render: (data, type, row, meta) => { // data: giá trị, type: kiểu xử lý , row là toàn bộ dữ liêu của hàng đó , meta là vị trị của ô đó  
                    return `<button type="button" class="btn btn-sm bg-secondary edit-stock" data-stock-id="${row.id}" data-toggle="modal" data-target="#StockEditModal">
                                 <i class="fas fa-edit mr-2"></i>  ${l('EditQuantity')}
                             </button>`;

                }
            }

        ]
    });
    console.log(_$stocksTable);
    console.log(_$stocksTable.context);
    console.log(_$stocksTable.selector);
    _$form.validate({
        rules: {
            Name: {
                required: true,
                maxlength: 50
            },
         
            Quantity: {
                required: true,
                number: true,
                min: 0
            },
            CategoryId: {
                required: true,
            }
        },
        messages: {
            Name: {
                required: "Vui lòng nhập tên sản phẩm",
                maxlength: "Tên sản phẩm không vượt quá 50 ký tự"
            },
           
            Quantity: {
                required: "Vui lòng nhập số lượng",
                number: "Số lượng phải là số",
                min: "Số lượng không được nhỏ hơn 0"
            },
            CategoryId: {
                required: "Vui lòng chọn danh mục",
            }
        }
    });
    // fix min
    $.validator.methods.min = function (value, element, param) {
        value = value.replace(',', '.');
        return this.optional(element) || parseFloat(value) >= param;
    };
   
    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var formData = new FormData(_$form[0]);
        for (let pair of formData.entries()) {
            console.log(pair[0] + ':', pair[1]);
        }
      
        abp.ui.setBusy(_$modal);
        $.ajax({
            url: abp.appPath + 'api/services/app/stock/Create',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success('Lưu thành công');
                _$stocksTable.ajax.reload();
            },
            error: function (xhr) {
                alert("Lỗi tạo sản phẩm: " + xhr.responseText);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal);
            }
        });
    });

    $(document).on('click', '.delete-stock', function () {
        var stockId = $(this).attr("data-stock-id");
        var stockName = $(this).attr('data-stock-name');

        deleteProduct(stockId, stockName);
    });

    function deleteProduct(stockId, stockName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                stockName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _stockService.delete({
                        id: stockId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$stocksTable.ajax.reload();
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

    $(document).on('click', '.edit-stock', function (e) {
        var stockId = $(this).attr("data-stock-id");
        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Stocks/EditModal?stockId=' + stockId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#StockEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#StockCreateModal"]', (e) => {
        $('.nav-tabs a[href="#stock-details"]').tab('show')
    });

    abp.event.on('stock.edited', (data) => {
        _$stocksTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search,#FilterByDate').on('click', (e) => {
        e.preventDefault();
        _$stocksTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$stocksTable.ajax.reload();
            return false;
        }
    });

    $(".filter-category").on('change', function (e) {
        e.preventDefault();
        _$productsTable.ajax.reload();
    })



    var _categoryService = abp.services.app.category;
    //console.log(_categoryService);
    _categoryService.getAll().done(function (data) {
        var selects = ['#CategoryId', '#CreateCategoryId'];

        selects.forEach(function (selector) {
            var $select = $(selector);
            if ($select.length) {
                $select.empty(); // Xóa các option cũ (nếu cần)
                $select.append($('<option/>', {
                    value: '',
                    text: '-- Chọn danh mục --'
                }));

                $.each(data.items, function (index, category) {
                    $select.append($('<option/>', {
                        value: category.id,
                        text: category.name
                    }));
                });
            }
        });
    });


})(jQuery);
