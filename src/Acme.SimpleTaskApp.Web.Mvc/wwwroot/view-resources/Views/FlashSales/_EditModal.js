(function ($) {
    var _flashSaleService = abp.services.app.flashSale,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#FlashSaleEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }
      var formData = _$form.serializeFormToObject();
      formData.IsActive = formData.IsActive === "1";

        
        abp.ui.setBusy(_$modal);
        $.ajax({
            url: abp.appPath + 'api/services/app/flashSale/update',
            type: 'POST',
            data: JSON.stringify(formData),
            contentType: 'application/json',
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success('Cập nhật thông tin FlashSale thành công');
              abp.event.trigger('flashsale.edited', formData);

            },
            error: function (xhr) {
                    abp.message.error('Có lỗi xảy ra, vui lòng thử lại.', 'Lỗi');
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

   
    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

})(jQuery);
