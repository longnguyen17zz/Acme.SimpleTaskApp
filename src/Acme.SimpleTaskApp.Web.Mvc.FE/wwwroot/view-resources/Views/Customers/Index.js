
   

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
            } else {
                alert("Số lượng sản phẩm vượt quá tồn kho!");
            }
        });
        $(document).on("click", "#decrease", function () {
            let value = parseInt($("#quantity").val()) || 1;
            if (value > 1) {
                $("#quantity").val(value - 1);
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
        });



        $(document).on("click", ".btn-buy", function () {
            const productId = $(this).data('product-id');
            const quantity = parseInt($('#quantity').val());
            console.log(productId);
            console.log(quantity);
            $.ajax({
                url: '/Carts/AddToCart',
                method: 'POST',
                data: { productId, quantity },
                success: function () {
                    // Gọi lại action Index và cập nhật vùng hiển thị giỏ hàng
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

        
    });

})(jQuery);
