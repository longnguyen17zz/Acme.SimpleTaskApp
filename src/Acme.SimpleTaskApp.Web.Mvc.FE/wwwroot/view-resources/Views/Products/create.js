(function ($) {
    $(function () {
        var _$form = $('#CreateProductForm');
        _$form.find('button[type=submit]')
            .click(function (e) {
                e.preventDefault();
                if (!_$form.valid()) {
                    return;
                }
                var formData = new FormData(_$form[0]);
                $.ajax({
                    url: abp.appPath + 'api/services/app/product/Create', 
                    type: 'POST',
                    data: formData,
                    processData: false, 
                    contentType: false, 
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