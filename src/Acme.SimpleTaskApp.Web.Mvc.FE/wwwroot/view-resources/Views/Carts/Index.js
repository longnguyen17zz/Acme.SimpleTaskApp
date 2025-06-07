(function ($) {
    $(function () {
        $(document).ready(function () {
            function updateTotalPrice() {
                let total = 0;
                $('tbody tr').each(function () {
                    const quantity = parseInt($(this).find('input[name="quantity"]').val());
                    const price = parseInt($(this).find('td:nth-child(5)').text().replace(/\D/g, ''));
                    total += quantity * price;

                    //$(".quantity-input-hidden").eq(index).val(quantity);
                });

                $('h4 strong').text(total.toLocaleString('vi-VN') + ' đ');
            }

            $('.increase').on('click', function (e) {
                e.preventDefault();
                const $row = $(this).closest('tr');
                const $input = $row.find('input[name="quantity"]');
                let quantity = parseInt($input.val());

                quantity++;
                $input.val(quantity);

                const price = parseInt($row.find('td:nth-child(5)').text().replace(/\D/g, ''));
                const newLineTotal = quantity * price;
                $row.find('td:nth-child(7)').text(newLineTotal.toLocaleString('vi-VN') + ' đ');

                updateTotalPrice();
            });

            $('.decrease').on('click', function (e) {
                e.preventDefault();
                const $row = $(this).closest('tr');
                const $input = $row.find('input[name="quantity"]');
                let quantity = parseInt($input.val());

                if (quantity > 1) {
                    quantity--;
                    $input.val(quantity);

                    const price = parseInt($row.find('td:nth-child(5)').text().replace(/\D/g, ''));
                    const newLineTotal = quantity * price;
                    $row.find('td:nth-child(7)').text(newLineTotal.toLocaleString('vi-VN') + ' đ');

                    updateTotalPrice();
                }
            });

            $('#cart-container').on('click', '.btn-delete-cart', function (e) {
                e.preventDefault();

                var cartItemId = $(this).data('id');

                if (confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?")) {
                    $.ajax({
                        url: '/Carts/RemoveFromCart',
                        method: 'POST',
                        data: { cartItemId: cartItemId },
                        success: function (partialHtml) {
                            //alert("Xóa thành công");
                            // Gán lại phần HTML cho #cart-container
                            //$('#cart-container').html(partialHtml);
                            location.reload();
                        },
                        error: function (xhr, status, error) {
                            console.log(xhr.responseText);
                            alert('Lỗi khi gửi yêu cầu xoá.');
                        }
                    });
                }
            });
        });
    });

})(jQuery);
