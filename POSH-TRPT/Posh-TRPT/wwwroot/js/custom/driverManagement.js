$(document).ready(function () {
    $('#detailDriver').hide();
    BindAllDriverListDatatable();

});

//function OnClickNextPage() {
//    document.getElementById('canid').addEventListener("click", function (event) {
//        event.preventDefault();
//    });
//    //return false;
//    $('#detailDriver').hide();
//    $('#listDriver').show();
//}

$("#canid").click(function () {
    $('#detailDriver').hide();
    $('#listDriver').show();

})

function BindAllDriverListDatatable() {
    $.ajax({
        url: "/DriverManager/GetDrivers",
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
            $('#tblDriver').DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,
                "columns": [
                    {
                        'data': 'id',
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { 'data': 'driverName' },
                    { 'data': 'email' },
                    { 'data': 'inspection_Expiry_Date' },
                    { 'data': 'mobile' },
                    { 'data': 'docStatus' },
                    {
                        'data': '',
                        'render': (data, type, row) => {
                            return `<a class="btn btn-warning" onclick="viewDriver(this)" id='${row.id}' href='#'><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;<a class="btn btn-danger" onclick="deleteDriver(this)" id='${row.id}'  href='#'><i class="fa-solid fa-trash"></i></a>`;
                        }
                    }
                ]


            });
        }
    });
}

function viewDriver(e) {

    BindAllStausListDropdown();
    BindAllRideCategoryListDropdown();
    $.ajax({
        url: "/DriverManager/GetDriverDetailById",
        type: "POST",
        dataType: "json",
        data: { userId: e.id },
        success: function (data) {
            $('#listDriver').hide();           
            $('#detailDriver').show();
            var nam = document.getElementById('rcDropError');
            nam.style.display = 'none';
            Object.entries(data).forEach(([key, value]) => {
                switch (key) {
                    case "user":
                        $('#bindStatusDropdown').val(value["docStatus"]); 
                        if (value["docStatus"].localeCompare("16069375-a542-4eea-b0af-6afa6272ef8b") == 0) {
                            document.getElementById("driverStatusUpdateSubmitBtnId").innerHTML = "Notify Driver";
                        }
                        else {
                            document.getElementById("driverStatusUpdateSubmitBtnId").innerHTML = "Submit";
                        }
                        
                        $("#userId").val(value["id"]);
                        $("#dname").val(value["driverName"]);
                        $('#bindStatusDropdown').val(value["docStatus"]); 
                        $("#demail").val(value["email"]);
                        $("#mobile").val(value["mobile"]);
                        $("#dob").val(new Date(value["dob"]).toLocaleString().split(',')[0]);
                        $("#docStatusId").val(value["docStatus"]);
                        $("#driverComment").val(value["comment"]);
                        $('#proImage').attr("src", value["profilePhoto"]);
                                             
                        break;
                    case "vehicleData":
                        $("#vId").val(value["id"]);
                        $("#vehicle_Identification_Number").val(value["vehicle_Identification_Number"]);
                        $("#make").val(value["make"]);
                        $("#model").val(value["model"]);
                        $("#year").val(value["year"]);
                        $("#color").val(value["color"]);
                        $("#Vehicle_Plate").val(value["vehicle_Plate"]);
                        $("#Vehicle_Inspection").val(value["inspection_Expiry_Date"]);
                        $("#bindRideCategoryDropdown").val(value["rideCategoryId"]);
                        break;
                    case "addressData":
                        $("#addrId").val(value["id"]);
                        $("#countryName").val(value["countryName"]);
                        $("#stateName").val(value["stateName"]);
                        $("#cityName").val(value["cityName"]);
                        $("#social_Security_Number").val(value["social_Security_Number"]);
                        break;
                    case "documentsData":
                        $("#docId").val(value["id"]);
                        $("#drivingLicenceDocName").attr("src", value["drivingLicenceDocName"]);
                        $("#insuarnceDocName").attr("src", value["insuarnceDocName"]);
                        $("#vehicleRegistrationDocName").attr("src", value["vehicleRegistrationDocName"]);
                        $("#vehicleInspectionDocName").attr("src", value["vehicleInspectionDocName"]);
                        break;
                }

            });




        }
    });

}

function statusRequired(selectObject) {
    
    var data = selectObject.value;
    switch (data) {
        case "57deeadb-b1c5-4273-a830-ed8d3b001f70":
            {
                document.getElementById("bindRideCategoryDropdown").required = "required";
                
                break;
            }
        default:
            document.getElementById("bindRideCategoryDropdown").required = "";

    }
}
function deleteDriver(e) {
    swal({
        title: "Do you want delete this Driver?",
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
                    url: "/DriverManager/DeleteUser",
                    type: "POST",
                    data: { id: e.id },
                    success: function (data) {
                        switch (data) {
                            case 1: swal("Driver deleted Successfully!", "", "success");
                                break;
                            case 0: swal("Opps,Driver deletion failed!", "", "error");
                                break;
                        }
                        BindAllDriverListDatatable();
                    },
                    error: function (jXHR, textStatus, errorThrown) {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                    }

                });
            }
        });
}

function BindAllRideCategoryListDropdown() {


    $.ajax({
        url: "/MasterTable/GetRideCategory",
        type: "Get",
        success: function (data) {
            $("#bindRideCategoryDropdown").empty().append($("<option></option>").val("0").html("Select Ride Category"));
            $.each(data, function (data, value) {

                $("#bindRideCategoryDropdown").append($("<option></option>").val(value.id).html(value.name));
            })

        }
    });


}

function BindAllStausListDropdown() {
    $.ajax({
        url: "/MasterTable/GetStatusList",
        type: "Get",
        success: function (data) {
            $("#bindStatusDropdown").empty().append($("<option></option>").val("0").html("Select Status"));
            $.each(data, function (data, value) {

                $("#bindStatusDropdown").append($("<option></option>").val(value.id).html(value.name));
            })

        }
    });


}

$('#driverStatusUpdateSubmitBtnId').click(function (e) {
    e.preventDefault();
    var result = {
        StatusId: $.trim($("#bindStatusDropdown").val()),
        RideCategoryId: $.trim($("#bindRideCategoryDropdown").val()), 
        Message: $.trim($("#driverComment").val()),
        UserId: $.trim($("#userId").val())
    }
    var nam = document.getElementById('statusDropError');
    var namrc = document.getElementById('rcDropError');
    namrc.style.display = 'none';
    if (result.StatusId === "0") {

        nam.innerHTML = "Status is required*";
        nam.style.display = 'block';
        nam.style.color = "red";
        document.getElementById('bindStatusDropdown').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;
    }
    else {
        nam.style.display = 'none';
    }
    if (result.StatusId === "57deeadb-b1c5-4273-a830-ed8d3b001f70" && (result.RideCategoryId == '' || result.RideCategoryId == "0")) {

        namrc.innerHTML = "Ride Category is required*";
        namrc.style.display = 'block';
        namrc.style.color = "red";
        document.getElementById('bindRideCategoryDropdown').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;
    }
    else {
        namrc.style.display = 'none';
        
    }
    $.ajax({
        
        url: "/DriverManager/SetUserApprovalStatus",
        type: "POST",
        data: { approvalStatus: result },
        success: function (data) {
            switch (data) {
                case true:
                    {
                        swal("Record has been updated Successfully!", "", "success");
                        BindAllStausListDropdown();
                        window.location = '/DriverManager/Index';
                        break;
                    }
                case false:
                    {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                        break;
                    }
            }
            
        },
        error: function (jXHR, textStatus, errorThrown) {
            swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
        }

    });
    return false;
});

