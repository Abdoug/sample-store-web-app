let datatable;

$(document).ready(function () {
    initDatatable();
});

const initDatatable = () => {
    datatable = $('#productsTable').DataTable({
        ajax: {
            url: "/Admin/Product/GetAll" 
        },
        columns: [
            { data: "title", width: "15%" },
            { data: "isbn", width: "15%" },
            { data: "price", width: "15%" },
            { data: "author", width: "15%" },
            { data: "category.name", width: "15%" },
            {
                data: "id",
                width: "15%",
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="Product/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil"></i> Edit</a>
                            <a href="#" data-url="Product/Delete?id=${data}" onclick="removeProduct('Product/Delete?id=${data}')" class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete</a>
                        </div>
                    `;
                }
            }
        ]
    });
};

const removeProduct = url => {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this product!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: 'DELETE',
                    url,
                    success: function (result) {
                        if (result.status) {
                            datatable.ajax.reload();
                            swal(result.message, {
                                icon: "success",
                            });
                        } else {
                            swal(result.message, {
                                icon: "error",
                            });
                        }
                    },
                    error: function (result) {
                        swal(result.message, {
                            icon: "error",
                        });
                    }
                });
            }
        });
}