using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using System.ComponentModel;
using Acme.SimpleTaskApp.Orders.Dto;
using System.Linq;
using static Acme.SimpleTaskApp.Orders.Dto.GetOrderDetailsOutput;
using OfficeOpenXml.Style;

public class OrderExcelExporter : ITransientDependency
{
    public async Task<byte[]> ExportToFileAsync(List<OrderDto> orders)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Nguyen Van Long");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Orders");
            // Header
            worksheet.Cells[1, 1].Value = "Tên khách hàng";
            worksheet.Cells[1, 2].Value = "Số điện thoại";
            worksheet.Cells[1, 3].Value = "Địa chỉ";
            worksheet.Cells[1, 4].Value = "Thông tin đơn hàng";
            worksheet.Cells[1, 5].Value = "Ngày đặt hàng";
            worksheet.Cells[1, 6].Value = "Tổng giá đơn";
            worksheet.Cells[1, 7].Value = "Trạng thái đơn hàng";

            worksheet.Row(1).Style.Font.Bold = true;

            // Data
            for (int i = 0; i < orders.Count; i++)
            {
                var p = orders[i];

                worksheet.Cells[i + 2, 1].Value = p.FullName;
                worksheet.Cells[i + 2, 2].Value = p.PhoneNumber;
                worksheet.Cells[i + 2, 3].Value = p.Address;
                worksheet.Cells[i + 2, 3].Style.WrapText = true; // Cho phép xuống dòng trong ô Excel
                worksheet.Cells[i + 2, 4].Value = string.Join("\n", p.Items.Select(i => $"- {i.ProductName} x{i.Quantity} ({i.Price:N0}đ)"));
                worksheet.Cells[i + 2, 4].Style.WrapText = true; // Cho phép xuống dòng trong ô Excel
                worksheet.Cells[i + 2, 5].Value = p.OrderDate.ToString("yyyy-MM-dd");
                worksheet.Cells[i + 2, 6].Value = p.TotalAmount;
                worksheet.Cells[i + 2, 7].Value = p.Status;
                for (int col = 1; col <= 7; col++)
                {
                    worksheet.Cells[i + 2, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[i + 2, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }
            // Auto fit columns
            //worksheet.Cells.AutoFitColumns();
            worksheet.Column(1).Width = 20; // Họ tên
            worksheet.Column(2).Width = 15; // SĐT
            worksheet.Column(3).Width = 30; // Địa chỉ
            worksheet.Column(4).Width = 50; // Thông tin đơn hàng (nội dung dài)
            worksheet.Column(5).Width = 18; // Ngày đặt
            worksheet.Column(6).Width = 15; // Tổng tiền
            worksheet.Column(7).Width = 20; // Trạng thái

            return await Task.FromResult(package.GetAsByteArray());
        }
    }
}
