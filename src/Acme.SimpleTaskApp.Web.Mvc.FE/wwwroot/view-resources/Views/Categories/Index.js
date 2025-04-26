(function ($) {
    var _categoryService = abp.services.app.category,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#CategoryCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#CategoriesTable');

    var _$categoriesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        listAction: {
            ajaxFunction: _categoryService.getPaged,
            inputFilter: function () {
                return $('#CategoriesSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$categorysTable.draw(false)
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
                data: 'code',
                orderable: false,
            },
            {
                targets: 2,
                data: 'name',
                orderable: false,
            },
            {
                targets: 3,
                data: null,
                className: 'text-center',
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-category" data-category-id="${row.id}" data-toggle="modal" data-target="#CategoryEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-category" data-category-id="${row.id}" data-category-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.validate({
        rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            Code: {
                required: true,
                maxlength: 6
            },
        },
        messages: {
            Name: {
                required: "Vui lòng nhập tên danh mục",
                maxlength: "Tên sản phẩm không vượt quá 50 ký tự"
            },
            Code: {
                required: "Vui lòng nhập mã danh mục",
                maxlength: "Mã danh mục không vượt quá 6 ký tự"
            }
        }
    });
    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var category = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        _categoryService.create(category).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            _$categoriesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-category', function () {
        var categoryId = $(this).attr("data-category-id");
        var categoryName = $(this).attr('data-category-name');

        deleteCategory(categoryId, categoryName);
    });

    function deleteCategory(categoryId, categoryName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                categoryName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _categoryService.delete({
                        id: categoryId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$categoriesTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-category', function (e) {
        var categoryId = $(this).attr("data-category-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Categories/EditModal?categoryId=' + categoryId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#CategoryEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#CategoryCreateModal"]', (e) => {
        $('.nav-tabs a[href="#category-details"]').tab('show')
    });

    abp.event.on('category.edited', (data) => {
        _$categoriesTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$categoriesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$categoriesTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
