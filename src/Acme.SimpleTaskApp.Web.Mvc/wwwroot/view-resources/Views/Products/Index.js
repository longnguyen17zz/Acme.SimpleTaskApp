(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#ProductCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductsTable');

    var _$productsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        listAction: {
            ajaxFunction: _productService.getAll,
            inputFilter: function () {
                return $('#ProductsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productsTable.draw(false)
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
                data: 'name',
                className: 'text-center',
            },
            {
                targets: 2,
                data: 'description',
                orderable: false,
                className: 'text-center',
            },
            {
                targets: 3,
                data: 'price',
                className: 'text-center',
            },
            {
                targets: 4,
                data: 'stockQuantity',
                className: 'text-center',
            },
            {
                targets: 5,
                data: 'images',
                className: 'text-center',
                render: function (data, type, row, meta) {
                    if (data) {
                        return `<img src="${data}" alt="Image" style="max-height: 60px;" />`;
                    } else {
                        return '<span class="text-muted">No image</span>';
                    }
                }
            },
            {
                targets: 6,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                className: 'text-center',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#ProductEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            //alert("Lỗi tạo sản phẩm!");
            return;
        }
        //var product = new FormData(_$form[0]);
        //var product = _$form.serializeFormToObject();

        //abp.ui.setBusy(_$modal);
        //_productService.create(product).done(function () {
        //    _$modal.modal('hide');
        //    _$form[0].reset();
        //    abp.notify.info(l('SavedSuccessfully'));
        //    _$productsTable.ajax.reload();
        //}).always(function () {
        //    abp.ui.clearBusy(_$modal);
        //});
        var formData = new FormData(_$form[0]);
        for (let pair of formData.entries()) {
            console.log(pair[0] + ':', pair[1]);
        }
        abp.ui.setBusy(_$modal);

        $.ajax({
            url: abp.appPath + 'api/services/app/product/Create', 
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.info('Lưu thành công');
                _$productsTable.ajax.reload();
            },
            error: function (xhr) {
                alert("Lỗi tạo sản phẩm: " + xhr.responseText);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal);
            }
        });
    });

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
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-product', function (e) {
        var productId = $(this).attr("data-product-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Products/EditModal?productId=' + productId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProductEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#ProductCreateModal"]', (e) => {
        $('.nav-tabs a[href="#user-details"]').tab('show')
    });

    abp.event.on('product.edited', (data) => {
        _$productsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$productsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$productsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
