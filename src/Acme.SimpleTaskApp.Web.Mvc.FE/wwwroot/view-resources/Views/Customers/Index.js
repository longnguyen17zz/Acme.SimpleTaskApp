
   

(function ($) {
    $(function () {
        $(document).ready(function () {
            $(document).on("click", ".product-card", function () {
                var id = $(this).data("id");
                window.location.href = '/Homepage/GetDetail/' + id;
            });

            $(".btn-login").on("click", function () {
                window.location.href = 'Account/Login'
            });

            
        });

        $(document).on("click", ".pagination .page-link", function (e) {
            e.preventDefault();
            var page = $(this).data("page");
            if (!page || $(this).closest(".page-item").hasClass("disabled")) return;
            var pageSize = $("#pagination-data").data("pagesize");
            var keyword = $("#pagination-data").data("keyword");
            var categoryId = $("#pagination-data").data("categoryid");

            $.ajax({
                url: '/Homepage/Index',
                data: {
                    SkipCount: (page - 1) * pageSize,
                    MaxResultCount: pageSize,
                    Keyword: keyword,
                    CategoryId: categoryId
                },
                success: function (result) {
                    $("#productList").html(result);
                }
            });
        });

        $(document).on("click", ".nav-tabs .nav-link", function () {
            $(".nav-tabs .nav-link").removeClass("active font-weight-bold");
            $(this).addClass("active font-weight-bold");

            const categoryId = $(this).data("categoryid");
            const pageSize = $("#pagination-data").data("pagesize");
            const keyword = $("#pagination-data").data("keyword");

            $.ajax({
                url: '/Homepage/Index',
                data: {
                    CategoryId: categoryId,
                    SkipCount: 0,
                    MaxResultCount: pageSize,
                    Keyword: keyword
                },
                success: function (result) {
                    $("#productList").html(result); // Cập nhật lại sản phẩm
                    $("#pagination-data").data("categoryid", categoryId); // Cập nhật lại giá trị hiện tại
                }
            });
        });

        $(document).on("click","#increase",function () {
            let value = parseInt($("#quantity").val()) || 1;
            let maxQuantity = parseInt($("#quantity").attr("max"));

            if (value < maxQuantity) {
                $("#quantity").val(value + 1);
                $("#buyNowQuantity").val(value + 1);
            } else {
                alert("Số lượng sản phẩm vượt quá tồn kho!");
            }
        });
        $(document).on("click", "#decrease", function () {
            let value = parseInt($("#quantity").val()) || 1;
            if (value > 1) {
                $("#quantity").val(value - 1);
                $("#buyNowQuantity").val(value - 1);
            }
        });

        $(document).on("input", "#quantity", function () {
            let value = parseInt($(this).val()) || 1;
            let maxQuantity = parseInt($(this).attr("max")); 
            
            if (value > maxQuantity) {
                $(this).val(maxQuantity);
                alert("Số lượng vượt quá tồn kho!");
            }
            if (value < 1) {
                $(this).val(1);
            }
            $("#buyNowQuantity").val($(this).val());
        });
        // Giỏ hàng
        $(document).on("click", ".shopping-cart, .shopping-cart-detail", function (event) {
            event.preventDefault();
            event.stopPropagation();

            const button = $(this);
            const productId = button.data("product-id");

            // Nếu tồn tại input #quantity thì lấy từ đó, ngược lại mặc định là 1
            const quantityInput = $('#quantity');
            const quantity = quantityInput.length > 0 ? parseInt(quantityInput.val()) : 1;

            if (!productId || isNaN(quantity) || quantity <= 0) {
                abp.message.warn("Thông tin sản phẩm hoặc số lượng không hợp lệ.");
                return;
            }
            const isFlashSale = $(this).data('is-flash-sale') === true || $(this).data('is-flash-sale') === "True";
            const salePrice = parseFloat($(this).data('sale-price'));

            const unitPrice = isFlashSale && salePrice > 0 ? salePrice : null; // gửi null nếu dùng giá gốc

            $.ajax({
                url: '/Carts/AddToCart',
                method: 'POST',
                data: { productId, quantity, unitPrice },
                success: function () {
                    // Tải lại nội dung giỏ hàng (nếu có container để hiển thị tóm tắt giỏ)
                    $('#cart-container').load('/Carts/Index');

                    abp.message.success('Đã thêm vào giỏ hàng');

                    $.get('/Carts/GetCartCount', function (count) {
                        $('#cart-badge').text(count);
                    });
                },
                error: function () {
                    alert("Vui lòng đăng nhập");
                }
            });
        });
       
        // Trang chi tiết đơn hàng
        $(document).on("click", ".order-success", function () {
            var orderId = $(this).data('order-id');

            abp.message.confirm(
                "Bạn chắc chắn đã nhận hàng thành công?",
                "Xác nhận đơn hàng",
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy(); // Hiển thị loading
                        abp.ajax({
                            url: abp.appPath + "api/services/app/Order/ConfirmDelivery",
                            type: "POST",
                            data: JSON.stringify({ orderId: orderId })
                        }).done(function () {
                            abp.notify.success("Đã xác nhận đơn hàng!");
                            setTimeout(function () {
                                location.reload();
                            }, 2000);
                            
                        }).fail(function () {
                            abp.notify.error("Có lỗi xảy ra khi xác nhận!");
                        }).always(function () {
                            abp.ui.clearBusy();
                        });
                    }
                }
            );
        });
    
    });


    // Lấy dữ liệu tinh thành 
        let allProvinces = [];
        // Lấy danh sách tỉnh ban đầu
        fetch('https://provinces.open-api.vn/api/p/')
      .then(res => res.json())
      .then(data => {
            allProvinces = data;
        const provinceSelect = document.getElementById('province');
        data.forEach(province => {
          const opt = document.createElement('option');
        opt.value = province.name;
        opt.textContent = province.name;
        opt.dataset.code = province.code;
        provinceSelect.appendChild(opt);
        });
      });
        let selectedProvinceCode = null;
        let selectedDistrictCode = null;
        // Khi chọn tỉnh
        document.getElementById('province')?.addEventListener('change', function () {
      const provinceName = this.value;
      const province = allProvinces.find(p => p.name === provinceName);
        const districtSelect = document.getElementById('district');
        const wardSelect = document.getElementById('ward');

        districtSelect.innerHTML = '<option value="">Chọn huyện</option>';
        wardSelect.innerHTML = '<option value="">Chọn xã</option>';

        if (!province) return;
        selectedProvinceCode = province.code;

        fetch(`https://provinces.open-api.vn/api/p/${selectedProvinceCode}?depth=2`)
        .then(res => res.json())
        .then(data => {
            data.districts.forEach(district => {
                const opt = document.createElement('option');
                opt.value = district.name;
                opt.textContent = district.name;
                opt.dataset.code = district.code;
                districtSelect.appendChild(opt);
            });
        });
    });
        // Khi chọn huyện
        document.getElementById('district')?.addEventListener('change', function () {
      const districtName = this.value;
        const districtSelect = document.getElementById('district');
        const selectedOption = districtSelect.options[districtSelect.selectedIndex];
        const districtCode = selectedOption.dataset.code;
        selectedDistrictCode = districtCode;

        const wardSelect = document.getElementById('ward');
        wardSelect.innerHTML = '<option value="">Chọn xã</option>';

        if (!districtCode) return;

        fetch(`https://provinces.open-api.vn/api/d/${districtCode}?depth=2`)
        .then(res => res.json())
        .then(data => {
            data.wards.forEach(ward => {
                const opt = document.createElement('option');
                opt.value = ward.name;
                opt.textContent = ward.name;
                wardSelect.appendChild(opt);
            });
        });
    });


    // Get Detail Product, Xử lý flashSale

})(jQuery);
