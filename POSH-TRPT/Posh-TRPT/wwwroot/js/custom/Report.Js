$(document).ready(function () {
    BindAllDriverListDropDown();
});

function BindAllDriverListDropDown() {
    $.ajax({
        url: "/DriverManager/GetDrivers",
        type: "Get",
        success: function (data) {
            $("#bindAllDriversData").empty().append($("<option></option>").val("0").html("Select Driver"));
            $.each(data, function (data, value) {
                $("#bindAllDriversData").append($("<option ps-2 border border-2></option>").val(value.id).html(value.driverName));
            });
        }
    });
}
function BindAllOrderStatusListDropDown() {
    $.ajax({
        url: "/DriverManager/GetAllOrdersForReport",
        type: "Get",
        success: function (data) {
            $("#bindAllStatusData").empty().append($("<option></option>").val("0").html("All"));
            debugger
            $.each(data, function (data, value) {
                debugger
                $("#bindAllStatusData").append($("<option ps-2 border border-2></option>").val(value.id).html(value.driverName));
            });
        }
    });
}
function BindAllFilterdOrderData(event)
{
    event.preventDefault();
   
    if ($("#StartDate").val() != "")
    {
        if ($("#EndDate").val() != "")
        {
            debugger;
            $("#loadershowhide").show();
            $("#eError").html("");
            $("#error").html("");
            var data = {
                startDate: $("#StartDate").val(),
                endDate: $("#EndDate").val(),
                statusType: $("#bindAllStatusData").val(),
                driverId: $("#bindAllDriversData").val(),
            };

            $.ajax({
                url: "/Report/GetFilteredDataOfOrders",
                type: "Get",
                data:
                {
                    startDate: data.startDate,
                    endDate: data.endDate,
                    statusType: data.statusType,
                    driverId: data.driverId
                },
                async: true,
                
                success: function (data) {
                    $("#table tbody").remove();
                    $("#table").show();
                    $("#loadershowhide").hide();
                    if (data != null) {
                        debugger;
                        // Assuming record.data is an array of objects
                        $('#tableCompletedRow').DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "bDestroy": true,
                            "searching": false,
                            "data": data.reportOrderData,
                            "columns": [{
                                'data': 'newDate'
                            },
                            {
                                'data': 'category'
                            },
                            {
                                'data': 'driver'
                            },
                            {
                                'data': 'rider'
                            },
                            {
                                'data': 'pickTime'
                            },
                            {
                                'data': 'dropOffTime'
                            },
                            {
                                'data': 'riderSourceName'
                            },
                            {
                                'data': 'destinationPlaceName'
                            },
                            {
                                'data': 'rideTotalPrice'
                            },
                            {
                                'data': 'tollFees'
                            },
                            ]
                        });
                        if (data.reportOrderData != null) {
                            $("#pdfFile").show();
                            $("#execlFile").show();
                            $("#pdfFile").attr("href", data.pdfURL);
                            $("#execlFile").attr("href", data.execlURL);
                        }
                        else {
                            debugger;
                            $("#pdfFile").hide();
                            $("#execlFile").hide();
                        }
                    }
                }
            });

        }
        else
        {
            $("#loadershowhide").hide();
            var nam = document.getElementById('eError');
            nam.innerHTML = "Please select end date*";
            nam.style.display = 'block';
            nam.style.color = "red";
            return false;
        }
    }
    else
    {
        $("#loadershowhide").hide();
        var nam = document.getElementById('error');
        nam.innerHTML = "Please select start date*";
        nam.style.display = 'block';
        nam.style.color = "red";
        return false;
    }
  
}



