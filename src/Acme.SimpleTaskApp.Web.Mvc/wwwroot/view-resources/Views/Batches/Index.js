(function ($) {
  var _batchService = abp.services.app.batch,
    l = abp.localization.getSource('SimpleTaskApp'),
    _$modal = $('#BatchCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#BatchsTable');
  console.log(_batchService);

  var _$batchsTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    processing: true,
    //pageLength: 5,
    //lengthMenu: [5, 10, 20, 50, 100], 
    listAction: {
      ajaxFunction: _batchService.getPaged,
      inputFilter: function () {
        var filter = $('#BatchsSearchForm').serializeFormToObject(true);
        console.log(filter);
        return filter;
      }
    },
    buttons: [
      {
        name: 'refresh',
        text: '<i class="fas fa-redo-alt"></i>',
        action: () => _$batchsTable.draw(false)
      }
    ],
    responsive: {
      details: {
        type: 'column'
      }
    },
    columnDefs: [
      {
        targets: 0,
        className: 'control',
        defaultContent: '',
        orderable: false,
      },
      {
        targets: 1,
        data: 'dateEntry',
        className: 'text-center',
      },
      {
        targets: 2,
        data: 'importer',
        orderable: false,
        className: 'text-center',
      },
      {
        targets: 3,
        data: null,
        orderable: false,
        autoWidth: false,
        defaultContent: '',
        className: 'text-center',
        render: (data, type, row, meta) => { // data: giá trị, type: kiểu xử lý , row là toàn bộ dữ liêu của hàng đó , meta là vị trị của ô đó  
          return `<div class="dropdown">
                         <button class="btn btn-sm btn-primary dropdown-toggle" type="button" id="actionDropdown_${row.id}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              ${l('Actions')}
                         </button>
                         <div class="dropdown-menu p-0" aria-labelledby="actionDropdown_${row.id}">
                             <button type="button" class="dropdown-item text-secondary edit-batch" data-batch-id="${row.id}" data-toggle="modal" data-target="#BatchEditModal">
                                 <i class="fas fa-edit mr-2"></i>  ${l('Edit')}
                             </button>
                             <div class="dropdown-divider m-0"></div>
                             <button type="button" class="dropdown-item text-danger delete-batch" data-batch-id="${row.id}" data-batch-name="${row.name}">
                                 <i class="fas fa-trash-alt mr-2"></i>  ${l('Delete')}
                             </button>
                         </div>
                     </div>`;

        }
      }

    ]
  });
  console.log(_$batchsTable);
 
  _$form.validate({
    rules: {
     
    },
    messages: {
     
    }
  });
 
 
  _$form.find('.save-button').on('click', (e) => {
    e.preventDefault();
    if (!_$form.valid()) {
      return;
    }
    //var formData = new FormData(_$form[0]);
    //for (let pair of formData.entries()) {
    //  console.log(pair[0] + ':', pair[1]);
    //}
    var formData = _$form.serializeFormToObject();
   
    abp.ui.setBusy(_$modal);
    $.ajax({
      url: abp.appPath + 'api/services/app/batch/create',
      type: 'POST',
      contentType: 'application/json',   // gửi JSON
      data: JSON.stringify(formData),   
      success: function () {
        _$modal.modal('hide');
        _$form[0].reset();
        abp.message.success('Lưu thành công');
        _$batchsTable.ajax.reload();
      },
      error: function (xhr) {
        alert("Lỗi tạo sản phẩm: " + xhr.responseText);
      },
      complete: function () {
        abp.ui.clearBusy(_$modal);
      }
    });
  });

  $(document).on('click', '.delete-batch', function () {
    var batchId = $(this).attr("data-batch-id");
    var batchName = $(this).attr('data-batch-name');

    deleteProduct(batchId, batchName);
  });

  function deleteProduct(batchId, batchName) {
    abp.message.confirm(
      abp.utils.formatString(
        l('AreYouSureWantToDelete'),
        batchName),
      null,
      (isConfirmed) => {
        if (isConfirmed) {
          _batchService.delete({
            id: batchId
          }).done(() => {
            abp.notify.info(l('SuccessfullyDeleted'));
            _$batchsTable.ajax.reload();
          }).fail((xhr) => {
            // chưa nhảy vào case check phân quyền 
            if (xhr.status === 403) {
              abp.message.warn("Bạn không có quyền xóa sản phẩm", "Thông báo")
            } else {
              abp.message.error('Có lỗi xảy ra khi xóa sản phẩm.', 'Lỗi');
            }
            abp.message.error('Có lỗi xảy ra khi xóa sản phẩm.', 'Lỗi');
          });
        }
      }
    );
  }

  $(document).on('click', '.edit-batch', function (e) {
    var batchId = $(this).attr("data-batch-id");
    e.preventDefault();
    abp.ajax({
      url: abp.appPath + 'Batches/EditModal?batchId=' + batchId,
      type: 'POST',
      dataType: 'html',
      success: function (content) {
        $('#BatchEditModal div.modal-content').html(content);
      },
      error: function (e) {
      }
    });
  });

  $(document).on('click', 'a[data-target="#BatchCreateModal"]', (e) => {
    $('.nav-tabs a[href="#batch-details"]').tab('show')
  });

  abp.event.on('product.edited', (data) => {
    _$productsTable.ajax.reload();
  });

  _$modal.on('shown.bs.modal', () => {
    _$modal.find('input:not([type=hidden]):first').focus();
  }).on('hidden.bs.modal', () => {
    _$form.clearForm();
  });

  $('.btn-search,#FilterByDate').on('click', (e) => {
    e.preventDefault();
    _$productsTable.ajax.reload();
  });

  $('.txt-search').on('keypress', (e) => {
    if (e.which == 13) {
      _$productsTable.ajax.reload();
      return false;
    }
  });

  $(".filter-category").on('change', function (e) {
    e.preventDefault();
    _$productsTable.ajax.reload();
  })




  //var _categoryService = abp.services.app.category;
  ////console.log(_categoryService);
  //_categoryService.getAll().done(function (data) {
  //  var selects = ['#CategoryId', '#CreateCategoryId'];

  //  selects.forEach(function (selector) {
  //    var $select = $(selector);
  //    if ($select.length) {
  //      $select.empty(); // Xóa các option cũ (nếu cần)
  //      $select.append($('<option/>', {
  //        value: '',
  //        text: '-- Chọn danh mục --'
  //      }));

  //      $.each(data.items, function (index, category) {
  //        $select.append($('<option/>', {
  //          value: category.id,
  //          text: category.name
  //        }));
  //      });
  //    }
  //  });
  //});


})(jQuery);
