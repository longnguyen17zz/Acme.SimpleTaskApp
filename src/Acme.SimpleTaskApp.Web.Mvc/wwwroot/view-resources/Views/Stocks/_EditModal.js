(function ($) {
    var _productService = abp.services.app.stock,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#StockEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var stock = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);

        $.ajax({
            url: abp.appPath + 'api/services/app/stock/UpdateStock',
            type: 'POST',
            data: JSON.stringify(stock), // Serialize object to JSON string
            contentType: 'application/json', // Tell server we're sending JSON
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success('Cập nhật số lượng sản phẩm thành công');
                abp.event.trigger('stock.edited', stock);
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
            StockQuantity: {
                required: true,
                number: true,
                min: 0
            },
          
        },
        messages: {
            
            StockQuantity: {
                required: "Vui lòng nhập số lượng",
                number: "Số lượng phải là số",
                min: "Số lượng không được nhỏ hơn 0"
            }
        }
    });
    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

})(jQuery);
