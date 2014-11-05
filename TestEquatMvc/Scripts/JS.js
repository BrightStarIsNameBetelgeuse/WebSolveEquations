function AddItem() {
    var title = $('#item_title').val();
    var description = $('#item_desc').val();
       
          $.ajax({
                  type: "POST",
                  url: "/Home/Index",
                  data: "title=" + title + "&description=" + description,
                  dataType: "html",
                 success: function (result) {
                         var domElement = $(result);
                         $('#items_list').append(domElement);
                     }
          });
  }