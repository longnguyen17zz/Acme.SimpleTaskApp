$(function () {
 $(document).ready(function () {
            $.ajax({
                url: '/api/services/app/DashBoard/GetDashboardData',
                method: 'GET',
                success: function (res) {
                    const data = res.result;

                    $('#totalRevenue').text(data.totalRevenue.toLocaleString('vi-VN') + ' ₫');
                    $('#totalOrders').text(data.totalOrders);
                    $('#totalCustomers').text(data.totalCustomers); 

                    // map 12 tháng theo dữ liệu 
                    const allMonths = [
                        'Thg 1', 'Thg 2', 'Thg 3', 'Thg 4', 'Thg 5', 'Thg 6',
                        'Thg 7', 'Thg 8', 'Thg 9', 'Thg 10', 'Thg 11', 'Thg 12'
                    ];
                    // Tạo map từ label -> value
                    const revenueMap = {};
                    data.revenuePerMonth.forEach(x => {
                        revenueMap[x.label] = x.value;
                    });
                    // Tạo labels và values theo đúng thứ tự 12 tháng
                    const revenueLabels = allMonths;
                    const revenueValues = allMonths.map(month => revenueMap[month] || 0);

                    new Chart(document.getElementById('revenueChart'), {
                        type: 'line',
                        data: {
                            labels: revenueLabels,
                            datasets: [{
                                label: 'Doanh thu (VND)',
                                data: revenueValues,
                                backgroundColor: 'rgba(76, 175, 80, 0.2)',
                                borderColor: '#4CAF50',
                                borderWidth: 2,
                                fill: true,
                                tension: 0.3, // làm mềm đường
                                pointBackgroundColor: '#4CAF50'
                            }]
                        },
                        options: {
                            responsive: true,
                            scales: {
                                y: {
                                    ticks: {
                                        callback: value => value.toLocaleString('vi-VN') + ' ₫'
                                    }
                                }
                            }
                        }
                    });

                    // Map đơn hàng theo 12 tháng
                    const orderMap = {};
                    data.ordersPerMonth.forEach(x => {
                        orderMap[x.label] = x.value;
                    });
                    const orderValues = allMonths.map(month => orderMap[month] || 0);

                    new Chart(document.getElementById('orderChart'), {
                        type: 'bar',
                        data: {
                            labels: allMonths,
                            datasets: [{
                                label: 'Số đơn hàng',
                                data: orderValues,
                                backgroundColor: 'rgba(54, 162, 235, 0.6)',
                                borderColor: 'rgba(54, 162, 235, 1)',
                                borderWidth: 1
                            }]
                        },
                        options: {
                            responsive: true,
                            scales: {
                                y: {
                                    beginAtZero: true,
                                    ticks: {
                                        precision: 0 // làm tròn đơn hàng
                                    }
                                }
                            }
                        }
                    });



                    new Chart(document.getElementById('orderStatusChart'), {
                        type: 'doughnut',
                        data: {
                            labels: ['Chờ xử lý', 'Đã hủy', 'Hoàn thành'],
                            datasets: [{
                                data: [
                                    data.orderStatusSummary.pending,
                                    data.orderStatusSummary.cancelled,
                                    data.orderStatusSummary.completed,

                                ],
                                backgroundColor: ['#facc15', '#f87171', '#4ade80']
                            }]
                        },
                        options: {
                            responsive: true
                        }
                    });
                },
                error: function () {
                    alert('Không thể tải dữ liệu thống kê.');
                }
            });
        });

})(jQuery);