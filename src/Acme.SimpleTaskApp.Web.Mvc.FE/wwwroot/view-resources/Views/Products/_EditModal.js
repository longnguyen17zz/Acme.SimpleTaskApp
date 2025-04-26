(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#ProductEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }
        var formData = new FormData(_$form[0]);
        for (let pair of formData.entries()) {
            console.log(pair[0] + ':', pair[1]);
        }
        var lang = abp.localization.currentLanguage.name;
        // đổi giá english thành vietnam lưu DB
        if (lang === "en") {
            let usdPrice = parseFloat(formData.get("Price"));
            let vndPrice = usdPrice * 25849; 
            formData.set("Price", Math.round(vndPrice)); // cập nhật lại giá
        }

        abp.ui.setBusy(_$modal);
        $.ajax({
            url: abp.appPath + 'api/services/app/product/UpdateProductData',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success('Cập nhật thông tin sản phẩm thành công');
                abp.event.trigger('product.edited', formData);
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    abp.message.warn('Bạn không có quyền sửa sản phẩm này!', 'Thông báo');
                } else {
                    abp.message.error('Có lỗi xảy ra, vui lòng thử lại.', 'Lỗi');
                }
            },
            complete: function () {
                abp.ui.clearBusy(_$modal);
            }
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

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
                min: 0
            },
            StockQuantity: {
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
            Images: {
                imageFile: "Ảnh không hợp lệ"
            },
            Price: {
                required: "Vui lòng nhập giá",
                localizedDecimal: "Giá phải là số hợp lệ",
                min: "Giá không được nhỏ hơn 0"
            },
            StockQuantity: {
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
    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });
    
})(jQuery);
