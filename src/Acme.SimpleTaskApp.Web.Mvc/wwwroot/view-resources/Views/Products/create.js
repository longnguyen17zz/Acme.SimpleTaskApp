(function ($) {
    $(function () {
        var _$form = $('#CreateProductForm');
        _$form.find('button[type=submit]')
            .click(function (e) {
                e.preventDefault();
                if (!_$form.valid()) {
                    return;
                }
                //var input = _$form.serializeFormToObject();
                var formData = new FormData(_$form[0]);
                //abp.services.app.product.create(formData)
                //    .done(function () {
                //        location.href = '/Products';
                //    });
                $.ajax({
                    url: abp.appPath + 'api/services/app/product/Create', // Đường dẫn API của ABP
                    type: 'POST',
                    data: formData,
                    processData: false, // Không xử lý dữ liệu
                    contentType: false, // Không đặt Content-Type (FormData tự xử lý)
                    success: function () {
                        alert("Thêm sản phẩm thành công!");
                        location.href = '/Products';
                    },
                    error: function (err) {
                        console.error("Lỗi gửi form:", err);
                        alert("Có lỗi xảy ra, vui lòng kiểm tra lại!");
                    }
                });
            });
    });
})(jQuery);