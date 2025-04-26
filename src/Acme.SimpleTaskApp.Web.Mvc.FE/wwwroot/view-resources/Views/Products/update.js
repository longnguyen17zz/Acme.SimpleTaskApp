$(document).on("click", ".view-image-btn", function () {
    var imageUrl = $(this).data("image"); // Lấy link ảnh từ button
    console.log("Image URL:", imageUrl);

    // Hiển thị ảnh trong modal
    $("#imageModal img").attr("src", imageUrl);
    $("#imageModal").modal("show");
});