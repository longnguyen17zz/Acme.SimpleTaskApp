(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('SimpleTaskApp'),
        _$modal = $('#ProductEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            alert("Validate data ");
            return;
        }

        //var product = _$form.serializeFormToObject();


        //abp.ui.setBusy(_$form);
        //_productService.update(product).done(function () {
        //    _$modal.modal('hide');
        //    abp.notify.info(l('SavedSuccessfully'));
        //    abp.event.trigger('product.edited', product);
        //}).always(function () {
        //    abp.ui.clearBusy(_$form);
        //});
        var formData = new FormData(_$form[0]);
        for (let pair of formData.entries()) {
            console.log(pair[0] + ':', pair[1]);
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
                abp.notify.info('Cập nhật thông tin sản phẩm thành công');
                abp.event.trigger('product.edited', formData);
            },
            error: function (xhr) {
                alert("Lỗi sửa thông tin sản phẩm: " + xhr.responseText);
                console.log(xhr.responseJSON);
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
