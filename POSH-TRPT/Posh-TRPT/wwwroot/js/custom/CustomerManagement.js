$(document).ready(function () {
    $('#EditCustomerForm').hide();
    BindAllCustomerListDatatable();
})

function BindAllCustomerListDatatable() {
    $.ajax({
        url: "/CustomerManager/GetCustomers",
        type: "Get",
        success: function (data) {
            columns = [];
            if (data.length > 0) {
                columnNames = Object.keys(data[0]);
                for (var i in columnNames) {
                    columns.push({
                        data: columnNames[i],
                        title: columnNames[i]
                    });
                }
            }
            $('#tblCustomer').DataTable({
                //"processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,
                "order": [[0, 'asc']],

                "columns": [
                    {
                        'data': 'id',
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { 'data': 'name' },
                    { 'data': 'email' },
                    { 'data': 'mobileNumber' },
                   
                    {
                        'data': '',
                        'render': (data, type, row) => {
                            //   return `<a class="btn btn-warning" onclick="updateCustomer(this)"  id='${row.id}' href='#'>update</a>&nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-danger" onclick="deleteCustomerData(this)" id='${row.id}' href='#'>Delete</a>`;

                            return `<a class="btn" onclick="updateCustomer(this)"  id='${row.id}' href='#'> <i class="fa fa-edit" style="font-size:24px;color : #501872"></i> </a><a class="btn" onclick="deleteCustomerData(this)" id='${row.id}' href='#'><i class="fa fa-trash" style="font-size:24px;color:red"></i></a>`;
                        }
                    }
                ]


            });
        }
    });
}

function updateCustomer(e) {
    $.ajax({
        url: "/CustomerManager/GetCustomerDetailById",
        type: "POST",
        data: { id: e.id },
        success: function (data) {
            $('#EditCustomerForm').show();
            $('#CustomerId').val(data.id);
            $('#CustomerName').val(data.name);
            $('#CustomerEmail').val(data.email);
            $('#CustomerMobile').val(data.mobileNumber);
        }
    });
}

function deleteCustomerData(e) {
    swal({
        title: "Do you want delete this Customer?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false,
        confirmButtonText: "Delete",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        showLoaderOnConfirm: true,
    },
        function (isConfirm) {
            if (isConfirm) {

        $.ajax({
            url: "/CustomerManager/DeleteCustomer",
            type: "POST",
            data: { id: e.id },
            success: function (data) {
                switch (data) {
                    case 1: swal("Customer deleted Successfully!", "", "success");
                        break;
                    case 0: swal("Opps,Customer deletion failed!", "", "error");
                        break;
                }
                BindAllCustomerListDatatable();
            },
            error: function (jXHR, textStatus, errorThrown) {
                swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
            }
        });
    }
        });
}

$('#EditCustomerForm').submit(function (e) {
    e.preventDefault();

    var result = {

        Id: $("#CustomerId").val(),
        Name: $('#CustomerName').val(),
        Email: $('#CustomerEmail').val(),
        MobileNumber: $('#CustomerMobile').val(),
    }

    $.ajax({
        url: "/CustomerManager/UpdateCustomerDetails",
        type: "POST",
        data: result,
        success: function (data) {

            swal("Details Updated Successfully !", "", "success")

            $('#EditCustomerForm').hide();

            BindAllCustomerListDatatable()
        },
        error: function (jXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }

    });

    return false;
});


