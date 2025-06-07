(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#ProductCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductsTable');
    console.log(_productService);

    var _$productsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        //pageLength: 5,
        //lengthMenu: [5, 10, 20, 50, 100], 
        listAction: {
            ajaxFunction: _productService.getPaged,
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
                render: function (data, type, row, meta) {
                    if (data) {
                        var lang = abp.localization.currentLanguage.name;
                        const VND_TO_USD_RATE = 25849;
                        const usd = data / VND_TO_USD_RATE;
                        if(lang=="vi")
                            return data.toLocaleString('vi-VN') + ' VND'; 
                        if (lang == "en")
                            return new Intl.NumberFormat('en-US', {
                                style: 'currency',
                                currency: 'USD',
                                minimumFractionDigits: 3, 
                                maximumFractionDigits: 3 
                            }).format(usd);
                    } else {
                        return '<span class="text-muted">No price</span>';
                    }
                }
            },
            {
                targets: 4,
                data: 'categoryName',
                orderable: false,
                className: 'text-center',
                render: function (data, type, row) {
                    return data ? data : 'Không có danh mục';
                }
            },
            {
                targets: 5,
                data: 'images',
                className: 'text-center',
                render: function (data, type, row, meta) {
                    if (data) {
                        return `<div style="height: 60px; width: 80px; margin: 0 auto;"><img src="${data}" alt="Image" style="height: 100%; width: 100%; object-fit: container;" /></div>`;
                    } else {
                        return '<span class="text-muted">No image</span>';
                    }
                }
            },
            {
                targets: 6,
                data: 'creationTime',
                className: 'text-center',
                render: function (data, type, row) {
                    if (!data) return '';
                    const date = new Date(data);
                    return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')
                        }/${date.getFullYear()} - ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
                }
            },
            {
                targets: 7,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                className: 'text-center',
                render: (data, type, row, meta) => { // data: giá trị, type: kiểu xử lý , row là toàn bộ dữ liêu của hàng đó , meta là vị trị của ô đó  
                    return `<div class="dropdown">
                         <button class="btn btn-sm btn-primary dropdown-toggle" type="button" id="actionDropdown_${row.id}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              ${l('Actions')}
                         </button>
                         <div class="dropdown-menu p-0" aria-labelledby="actionDropdown_${row.id}">
                             <button type="button" class="dropdown-item text-secondary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#ProductEditModal">
                                 <i class="fas fa-edit mr-2"></i>  ${l('Edit')}
                             </button>
                             <div class="dropdown-divider m-0"></div>
                             <button type="button" class="dropdown-item text-secondary view-product-detail" data-tour-id="${row.id}" data-tour-name="${row.name}">
                                 <i class="fas fa-eye mr-2"></i>  ${l('Details')}
                             </button>
                             <div class="dropdown-divider m-0"></div>
                             <button type="button" class="dropdown-item text-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}">
                                 <i class="fas fa-trash-alt mr-2"></i>  ${l('Delete')}
                             </button>
                         </div>
                     </div>`;

                }
            }

        ]
    });
    console.log(_$productsTable);
    console.log(_$productsTable.context);
    console.log(_$productsTable.selector);
    _$form.validate({
        rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            Images: {
                imageFile: {
                    allowedTypes: ['jpg', 'jpeg', 'png', 'webp'],
                    maxSize: 1 * 1024 * 1024,
                }
            },
            Price: {
                required: true,
                localizedDecimal: true,
                min: true
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
            Images: {
                imageFile: "Ảnh không hợp lệ"
            },
            Price: {
                required: "Vui lòng nhập giá",
                localizedDecimal:"Giá phải là số hợp lệ ",
                min: "Giá không được nhỏ hơn 0"
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
    // hỗ trợ nhập giá theo ngôn ngữ
    $.validator.addMethod("localizedDecimal", function (value, element) {
        var lang = abp.localization.currentLanguage.name;
        if (!value) return true;
        if (lang === "vi") {
            return /^-?\d{1,3}(?:\.\d{3})*(?:,\d+)?$|^\d+(?:,\d+)?$/.test(value);
        }
        if (lang === "en") {
            return /^-?\d{1,3}(?:,\d{3})*(?:\.\d+)?$|^\d+(?:\.\d+)?$/.test(value);
        }
        return false;
    }, "Giá phải là số hợp lệ ");
    // hỗ trợ kiểm tra ảnh
    $.validator.addMethod("imageFile", function (value, element, param) {
        const file = element.files[0];
        if (!file) return true; 

        const maxSize = param.maxSize || 2 * 1024 * 1024;
        const allowedTypes = param.allowedTypes || ['jpg', 'jpeg', 'png', 'gif'];
        const extension = file.name.split('.').pop().toLowerCase();

        if (!allowedTypes.includes(extension)) {
            return false;
        }

        if (file.size > maxSize) {
            return false;
        }

        return true;
    }, function (params, element) {
        const extensions = params.allowedTypes.join(', ').toUpperCase();
        return `Vui lòng chọn ảnh (${extensions}) nhỏ hơn ${params.maxSize / 1024 / 1024}MB`;
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var formData = new FormData(_$form[0]);
        for (let pair of formData.entries()) {
            console.log(pair[0] + ':', pair[1]);
        }
        var lang = abp.localization.currentLanguage.name;
        if (lang === "en") {
            let usdPrice = parseFloat(formData.get("Price"));
            let vndPrice = usdPrice * 25849;
            formData.set("Price", Math.round(vndPrice)); // cập nhật lại giá
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
                abp.message.success('Lưu thành công');
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
        $('.nav-tabs a[href="#product-details"]').tab('show')
    });

    abp.event.on('product.edited', (data) => {
        _$productsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search,#FilterByDate').on('click', (e) => {
        e.preventDefault();
        _$productsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$productsTable.ajax.reload();
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
