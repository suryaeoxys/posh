
var columns = [];
$(document).ready(function () {
    var methodName = window.location.href.split("/")
    var name = methodName[methodName.length - 1].replaceAll("#", '')





    switch (name) {

        case 'Index': BindAllCountryListDatatable(); break;
        case 'City': BindAllCityListDatatable(); break;
        case 'State':
            BindAllStateListDatatable(); break;
        case 'RideCategory': BindAllRideCategoryListDatatable(); break;
        case 'CategoryPrice': BindAllCategoryPriceListDatatable(); break;
    }
    // Use to Bind the State list with thier record in datatable
    // BindAllCountryListDatatable();

    // use to submit the new state into the datatable.


});



//if (maxInput<minInput) {
//    cpMnFareError.innerHTML = "Maximum fare must be greater than minimum fare";
//    cpMnFareError.style.display = 'block';
//    cpMnFareError.style.color = "red";
//} else if (minInput > maxInput) {    
//    cpMinFareError.innerHTML = "Minimum fare must be less than maximum fare";
//    cpMinFareError.style.display = 'none';
//    cpMinFareError.style.color = "red";
//} else {
//    // do something if the first input is less than the second
//}

//=================

function checkValuesForMin() {
    var minInput = document.getElementById("Minimum_Fare").value;
    var maxInput = document.getElementById("Maximum_Fare").value;
        var cpMnFareError = document.getElementById('cpMinFareError');
        if (parseFloat(minInput) > parseFloat(maxInput)) {
            cpMnFareError.innerHTML = "Min fare should be less than or equal to Max fare";
            cpMnFareError.style.display = 'block';
            cpMnFareError.style.color = "red";
            return false;
        }
        else {
            cpMnFareError.innerHTML = "";
            cpMnFareError.style.display = 'none';
            cpMnFareError.style.color = "red";
            return true;
        }
    }
function checkValuesForMax() {
    var minInput = document.getElementById("Minimum_Fare").value;
    var maxInput = document.getElementById("Maximum_Fare").value;
        var cpMxFareError = document.getElementById('cpMaxFareError');
        if (parseFloat(maxInput) < parseFloat(minInput)) {
            cpMxFareError.innerHTML = "Max fare should be greater than or equal to Min fare";
            cpMxFareError.style.display = 'block';
            cpMxFareError.style.color = "red";
            return false;
        }
        else {
            cpMxFareError.innerHTML = "";
            cpMxFareError.style.display = 'none';
            cpMxFareError.style.color = "red";
            return true;
        }
    }


// **************Start      Country Related functionality******************//

function BindAllCountryListDatatable() {

    $.ajax({
        url: "/MasterTable/GetCountry",
        type: "Get",
        success: function (data) {
            columns = [];
            $("#newCountryAdd").hide();
            if (data.length > 0) {
                columnNames = Object.keys(data[0]);
                for (var i in columnNames) {
                    columns.push({
                        data: columnNames[i],
                        title: columnNames[i]
                    });
                }
            }
            $('#datatble_CountryList').DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,

                "columns": [
                    { 'data': 'name' },
                    {
                        'data': '',
                        'render': (data, type, row) => {
                            return `<a class="btn btn-warning" onclick="updateCountrye(this)"  id='${row.id}' href='#'><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-danger"  onclick="deleteCountrye(this)" id='${row.id}' href='#'><i class="fa-solid fa-trash"></i></a>`;
                        }
                    }
                ]


            });
        }
    });


}

$("#AddButton").click(function () {
    $("#EditButton").val("Save");
    $("#country_id").val('');
    $("#CountryName").val('');
    $("#newCountryAdd").show();
})

$("#CloseButton").click(function () {
    $("#EditButton").val("Save");
    $("#country_id").val('');
    $("#CountryName").val('');
    $("#newCountryAdd").hide();
})

$('#newCountryAddForm').submit(function (e) {
    e.preventDefault();
    var result = {
        Id: $.trim($("#country_id").val()),
        Name: $.trim($("#CountryName").val())
    }
    var nam = document.getElementById('nameCountryError');
    if (result.Name === '') {

        nam.innerHTML = "Country is required*";
        nam.style.display = 'block';
        nam.style.color = "red";
        document.getElementById('CountryName').addEventListener("click", function (event) {
            event.preventDefault();
        });
        $("#CountryName").val('');
        return false;
    }
    else if (result.Name.length < 3) {
        nam.innerHTML = "Minimum 3 characters required*";
        nam.style.display = 'block';
        nam.style.color = "red";
        document.getElementById('CountryName').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;
    }
    else {
        nam.style.display = 'none';
        $.ajax({
            url: "/MasterTable/AddCountry",
            type: "POST",
            data: { result1: result },
            success: function (data) {
                switch (data.data) {
                    case 1: swal("Country Name Added Successfully!", "", "success");
                        break;
                    case 2: swal("Country Name Updated Successfully !", "", "success");
                        break;
                    case 3: swal("Country Name Already Exist!", "", "warning");
                        break;
                }
                BindAllCountryListDatatable();

            },
            error: function (jXHR, textStatus, errorThrown) {
                swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");

            }

        });
        return false;
    }

});

function updateCountrye(e) {
    $("#CountryName").val(e.parentNode.parentNode.childNodes[0].innerText);
    $("#country_id").val(e.id);
    $("#EditButton").val("Update");
    $("#newCountryAdd").show();
}

function deleteCountrye(e) {
    swal({
        title: "Do you want delete this Country Name?",
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
                    url: "/MasterTable/DeleteCountry",
                    type: "POST",
                    data: { id: e.id },
                    success: function (data) {
                        switch (data) {
                            case 1: swal("Country Name deleted Successfully!", "", "success");
                                break;
                            case 0: swal("Opps,Country Name deletion failed!", "", "error");
                                break;
                        }
                        BindAllCountryListDatatable();
                    },
                    error: function (jXHR, textStatus, errorThrown) {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                    }

                });
            }
        });
}




// **************End      Country Related functionality******************//





function BindAllStateListDatatable() {


    $.ajax({
        url: "/MasterTable/GetState",
        type: "Get",
        success: function (data) {
            $("#newStateAdd").hide();
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
            $('#datatble_CountryList').DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,
                "columns": [
                    // {'data':'countryName'},
                    { 'data': 'country.name' },
                    { 'data': 'name' },
                    {
                        'data': '',
                        'render': (data, type, row) => {
                            return `<a class="btn btn-warning" onclick="updateState(this)"  id='${row.id}' href='#'><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-danger" onclick="deleteState(this)" id='${row.id}' href='#'><i class="fa-solid fa-trash"></i></a>`;
                        }
                    }
                ]


            });
        }
    });

}
function CloseStateForm() {
    $("#state_id").val('');
    $("#StateName").val('');
    $("#bindstateCountryDropdown").val(0);
    $("#EditButton").val("Save");
    $("#newStateAdd").hide();
}
function OpenStateForm(id) {

    $.ajax({
        url: "/MasterTable/GetCountry",
        type: "Get",
        success: function (data) {

            $("#bindstateCountryDropdown").empty().append($("<option></option>").val("0").html("Select Country"));
            $.each(data, function (data, value) {

                $("#bindstateCountryDropdown").append($("<option></option>").val(value.id).html(value.name));

                if (id != null) {
                    $("#bindstateCountryDropdown").val(id);
                    $("#EditButton").val("Update");

                }
                else {
                    $("#state_id").val('');
                    $("#StateName").val('');
                    $("#bindstateCountryDropdown").val(0);
                    $("#EditButton").val("Save");
                }

            })
            $("#newStateAdd").show();

        }
    });


}


// use to submit the new state into the datatable.
function getChange() {

    var conBerror = document.getElementById('conBinderror');
    var stateError = document.getElementById('errState');
    var result = {
        CountryId: $.trim($("#bindstateCountryDropdown").val()),
        Name: $.trim($("#StateName").val()),
        Id: $.trim($("#state_id").val())
    }
    if (result.CountryId === "0") {
        conBerror.innerHTML = "Please select country name*";
        conBerror.style.display = 'block';
        conBerror.style.color = "red";
        document.getElementById('bindstateCountryDropdown').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;

    } else {
        conBerror.style.display = 'none';

        if (result.CountryId !== "0" && result.Name === '') {
            stateError.innerHTML = "State is required*";
            stateError.style.display = 'block';
            stateError.style.color = "red";
            document.getElementById('StateName').addEventListener("click", function (event) {
                event.preventDefault();
            });
            $("#StateName").val('');
            return false;
        }
        else if (result.CountryId !== "0" && result.Name.length < 3) {
            stateError.innerHTML = "Minimum 3 characters required*";
            stateError.style.display = 'block';
            stateError.style.color = "red";
            document.getElementById('StateName').addEventListener("click", function (event) {
                event.preventDefault();
            });
            $("#StateName").val('');
            return false;
        }
        stateError.style.display = 'none';
        return true;
    }
}
$('#newStateAddForm').submit(function (e) {
    e.preventDefault();
    var resu = getChange();
    if (resu === true) {
        var result = {
            CountryId: $("#bindstateCountryDropdown").val(),
            Name: $("#StateName").val(),
            StateCode: $("#StateCodeName").val(),
            Id: $("#state_id").val()
        }

        $.ajax({
            url: "/MasterTable/AddState",
            type: "POST",
            data: { result: result },
            success: function (data) {
                switch (data) {
                    case 1: swal("State Name Added Successfully!", "", "success");
                        break;
                    case 2: swal("State Name Updated Successfully !", "", "success");
                        break;
                    case 3: swal("State Name Already Exist!", "", "warning");
                        break;
                }

                BindAllStateListDatatable();
            },
            error: function (jXHR, textStatus, errorThrown) {
                swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
            }

        });
        return false;
    }
    else {
        $('#newStateAddForm').submit(function (e) {
            e.preventDefault();
        });
    }

});

function updateState(e) {

    $.ajax({
        url: "/MasterTable/GetStateDetailById",
        type: "POST",
        data: { id: e.id },
        success: function (data) {

            OpenStateForm(data.countryId);

            $("#StateName").val(data.name);
            $("#state_id").val(e.id);

        }
    });

}


function deleteState(e) {
    swal({
        title: "Do you want delete this State?",
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
                    url: "/MasterTable/DeleteState",
                    type: "POST",
                    data: { id: e.id },
                    success: function (data) {
                        switch (data) {
                            case 1: swal("State Name deleted Successfully!", "", "success");
                                break;
                            case 0: swal("Opps,State Name deletion failed!", "", "error");
                                break;
                        }
                        BindAllStateListDatatable();
                    },
                    error: function (jXHR, textStatus, errorThrown) {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                    }

                });
            }
        });
}






function BindAllCityListDatatable() {


    $.ajax({
        url: "/MasterTable/GetCity",
        type: "Get",
        success: function (data) {
            if (data.length > 0) {
                columnNames = Object.keys(data[0]);
                for (var i in columnNames) {
                    columns.push({
                        data: columnNames[i],
                        title: columnNames[i]
                    });
                }
            }

            $("#newCityAdd").hide();

            $('#datatble_CityList').DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,
                "columns": [{
                    'data': 'state.country.name', "name": "Country Name", "title": "Country Name"
                },
                {
                    'data': 'state.name', "name": "State Name", "title": "State Name"
                },
                {
                    'data': 'name', "name": "City Name", "title": "City Name"
                },
                {
                    'data': '',
                    'render': (data, type, row) => {
                        return `<a class="btn btn-warning" onclick="updateCity(this)"  id='${row.id}' href='#'><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-danger" onclick="deleteCity(this)" id='${row.id}' href='#''><i class="fa-solid fa-trash"></i></a>`;
                    }
                }
                ]


            });
        }
    });

}

function CloseCityForm(result) {
    $("#bindStateDropdown").val(0);
    $("#bindCountryDropdown").val(0);
    $("#city_id").val('');
    $("#CityName").val('');
    $("#EditButton").val("Save");
    $("#conBindCiterror").html("");

    $("#newCityAdd").hide();
}


function OpenCityForm(result) {

    $.ajax({
        url: "/MasterTable/GetCountry",
        type: "Get",
        success: function (data) {
            $("#bindCountryDropdown").empty().append($("<option></option>").val("0").html("Select Country"));

            $.each(data, function (data, value) {

                $("#bindCountryDropdown").append($("<option></option>").val(value.id).html(value.name));


            })

            if (result != null) {
                $("#bindCountryDropdown").val(result.state.countryId);
                $("#EditButton").val("Update");


                $.ajax({
                    url: "/MasterTable/GetState",
                    type: "POST",
                    data: {
                        Id: result.state.countryId
                    },
                    success: function (data) {


                        $("#bindStateDropdown").empty().append($("<option></option>").val("0").html("Select State"));

                        $("#bindStateDropdown").empty().append($("<option></option>").val("0").html("Select State"));
                        $.each(data, function (data, value) {
                            $("#bindStateDropdown").append($("<option></option>").val(value.id).html(value.name));
                        })
                        if (result.state.id != null) {
                            $("#bindStateDropdown").val(result.state.id);
                            $("#EditButton").val("Update");

                        } else {
                            $("#bindStateDropdown").val(0);
                            $("#bindCountryDropdown").val(0);
                            $("#city_id").val('');
                            $("#CityName").val('');
                            $("#EditButton").val("Save");
                        }

                    }
                });


            } else {
                $("#bindStateDropdown").val(0);
                $("#bindCountryDropdown").val(0);
                $("#city_id").val('');
                $("#CityName").val('');
                $("#EditButton").val("Save");
            }


            $("#newCityAdd").show();

        }
    });


}



$('#bindCountryDropdown').change(function (e) {


    $.ajax({
        url: "/MasterTable/GetState",
        type: "POST",
        data: {
            Id: e.currentTarget.value
        },
        success: function (data) {
            $("#bindCityDropdown").empty().append($("<option></option>").val("0").html("Select City"));
            $("#bindStateDropdown").empty().append($("<option></option>").val("0").html("Select State"));
            $.each(data, function (data, value) {

                $("#bindStateDropdown").append($("<option></option>").val(value.id).html(value.name));
            })

        }
    });


});


$('.bindStateDropdown').change(function (e) {


    $.ajax({
        url: "/MasterTable/GetCityByStateId",
        type: "POST",
        data: {
            Id: e.currentTarget.value
        },
        success: function (data) {

            $("#bindCityDropdown").empty().append($("<option></option>").val("0").html("Select City"));
            $.each(data, function (data, value) {

                $("#bindCityDropdown").append($("<option></option>").val(value.id).html(value.name));
            })

        }
    });


});



function updateCity(e) {

    $.ajax({
        url: "/MasterTable/GetCityDetailById",
        type: "POST",
        data: {
            id: e.id
        },
        success: function (data) {
            //OpenCityForm(data.state.countryId);
            OpenCityForm(data);


            $("#CityName").val(data.name);
            $("#city_id").val(e.id);

        }
    });

}


function deleteCity(e) {
    swal({
        title: "Do you want delete this City Name?",
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
                    url: "/MasterTable/DeleteCity",
                    type: "POST",
                    data: {
                        id: e.id
                    },
                    success: function (data) {
                        switch (data) {
                            case 1: swal("City Name deleted Successfully!", "", "success");
                                break;
                            case 0: swal("Opps,City Name deletion failed!", "", "error");
                                break;
                        }
                        BindAllCityListDatatable();
                    },
                    error: function (jXHR, textStatus, errorThrown) {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                    }

                });
            }
        });
}
function getCityChange() {
    var conConerror = document.getElementById('conBindConerror');
    var conStaError = document.getElementById('conBindStaerror');
    var conCitError = document.getElementById('conBindCiterror');
    var result = {
        CountryId: $.trim($("#bindCountryDropdown").val()),
        StateId: $.trim($("#bindStateDropdown").val()),
        CityName: $.trim($("#CityName").val())
    }
    if (result.CountryId === "0") {
        conConerror.innerHTML = "Please select country*";
        conConerror.style.display = 'block';
        conConerror.style.color = "red";
        document.getElementById('bindCountryDropdown').addEventListener("click", function (event) {
            event.preventDefault();
        });
        $("#CityName").val('');
        return false;
    }
    else if (result.StateId === "0") {
        conStaError.innerHTML = "Please select state*";
        conStaError.style.display = 'block';
        conStaError.style.color = "red";
        document.getElementById('bindStateDropdown').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;
    }
    else if (result.CountryId != "0") {
        conConerror.style.display = 'none';
        if (result.StateId != "0") {
            conStaError.style.display = 'none';
            if (result.CityName != '' && result.CityName.length < 3) {
                conCitError.innerHTML = "Minimum 3 characters required*";
                conCitError.style.display = 'block';
                document.getElementById('CityName').addEventListener("click", function (event) {
                    event.preventDefault();
                });
                return false;
            } else {
                if (result.CityName === '') {
                    conCitError.innerHTML = "City is required*";
                    conCitError.style.display = 'block';
                    conCitError.style.color = "red";
                    document.getElementById('CityName').addEventListener("click", function (event) {
                        event.preventDefault();
                    });
                    $("#CityName").val('');
                    return false;
                }
                else {
                    conCitError.style.display = 'none';
                    return true;
                }
            }
        }
    }
    else if (result.CityName === '') {

        conCitError.innerHTML = "State is required*";
        conCitError.style.display = 'block';
        conCitError.style.color = "red";
        document.getElementById('StateName').addEventListener("click", function (event) {
            event.preventDefault();
        });
        $("#CityName").val('');
        return false;
    }
    else if (result.CityName != '') {
        conCitError.style.display = 'none';
        return true;
    }
    else if (result.CityName.length < 3) {
        conCitError.innerHTML = "Minimum 3 characters required*";
        conCitError.style.display = 'block';
        conCitError.style.color = "red";
        document.getElementById('StateName').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;
    }
    else {
        conStaError.style.display = 'none';
        conConerror.style.display = 'none';
        return true;
    }

}
$('#newCityAddForm').submit(function (e) {
    e.preventDefault();
    var res = getCityChange();
    if (res) {
        var result = {
            Id: $("#city_id").val(),
            Name: $("#CityName").val(),
            StateId: $("#bindStateDropdown").val()
        };
        $.ajax({
            url: "/MasterTable/AddCity",
            type: "POST",
            data: {
                result: result
            },
            success: function (data) {
                switch (data) {
                    case 1: swal("City Name Added Successfully!", "", "success");
                        break;
                    case 2: swal("City Name Updated Successfully !", "", "success");
                        break;
                    case 3: swal("City Name Already Exist!", "", "warning");
                        break;
                }

                BindAllCityListDatatable();
            },
            error: function (jXHR, textStatus, errorThrown) {
                swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
            }

        });
        return false;
    }
    else {
        $('#newCityAddForm').submit(function (e) {
            e.preventDefault();
        });
    }



});


$('#newRideCategoryAddForm').submit(function (e) {
    e.preventDefault();

    var result = {
        Id: $.trim($("#RideCategory_id").val()),
        Name: $.trim($("#RideCategoryName").val())
    }
    var nam = document.getElementById('rideError');
    if (result.Name === '') {
        nam.innerHTML = "Ride Category is required*";
        nam.style.display = 'block';
        nam.style.color = "red";
        document.getElementById('RideCategoryName').addEventListener("click", function (event) {
            event.preventDefault();
        });
        return false;
    }
    else {
        nam.style.display = 'none';

        $.ajax({
            url: "/MasterTable/AddRideCategory",
            type: "POST",
            data: { result1: result },
            success: function (data) {
                switch (data) {
                    case 1: swal("Ride Category Name Added Successfully!", "", "success");
                        break;
                    case 2: swal("Ride Category Name  Updated Successfully !", "", "success");
                        break;
                    case 3: swal("Ride Category Name  Already Exist!", "", "warning");
                        break;
                }

                BindAllRideCategoryListDatatable();

            },
            error: function (jXHR, textStatus, errorThrown) {
                swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
            }

        });
        return false;
    }
});

function CloseRideCategoryForm() {
    $("#EditButton").val("Save");
    $("#RideCategory_id").val('');
    $("#RideCategoryName").val('');
    $("#newRideCategoryAdd").hide();
}


function OpenRideCategoryForm() {
    $("#EditButton").val("Save");
    $("#RideCategory_id").val('');
    $("#RideCategoryName").val('');
    $("#newRideCategoryAdd").show();
}

function BindAllRideCategoryListDatatable() {


    $.ajax({
        url: "/MasterTable/GetRideCategory",
        type: "Get",
        success: function (data) {
            $("#newRideCategoryAdd").hide();
            if (data.length > 0) {
                columnNames = Object.keys(data[0]);
                for (var i in columnNames) {
                    columns.push({
                        data: columnNames[i],
                        title: columnNames[i]
                    });
                }
            }
            $('#datatble_RideCategoryList').DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,

                "columns": [
                    { 'data': 'name' },
                    {
                        'data': '',
                        'render': (data, type, row) => {
                            return `<a class="btn btn-warning" onclick="updateRideCategory(this)"  id='${row.id}' href='#'><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-danger"  onclick="deleteRideCategory(this)" id='${row.id}' href='#'><i class="fa-solid fa-trash"></i></a>`;
                        }
                    }
                ]


            });
        }
    });


}


function updateRideCategory(e) {
    $("#RideCategoryName").val(e.parentNode.parentNode.childNodes[0].innerText);
    $("#RideCategory_id").val(e.id);
    $("#EditButton").val("Update");
    $("#newRideCategoryAdd").show();
}
function deleteRideCategory(e) {
    swal({
        title: "Do you want delete this Ride Category Name?",
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
                    url: "/MasterTable/DeleteRideCategory",
                    type: "POST",
                    data: { id: e.id },
                    success: function (data) {
                        switch (data) {
                            case 1: swal("Ride Category Name deleted Successfully!", "", "success");
                                break;
                            case 0: swal("Opps,Ride Category Name deletion failed!", "", "error");
                                break;
                        }
                        BindAllRideCategoryListDatatable();
                    },
                    error: function (jXHR, textStatus, errorThrown) {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                    }

                });
            }
        });
}
// ************** Start Category Price List  functionality-******************//

function CloseCategoryPriceForm() {
    $("#BaseFare").val(''),
        $("#Cancel_Penalty").val(''),
        $("#Cost_Per_Mile").val(''),
        $("#Cost_Per_Minute").val(''),
        $("#Maximum_Fare").val(''),
        $("#Minimum_Fare").val(''),
        $("#Sched_Cancel_Penalty").val(''),
        $("#Sched_Ride_Minimum_Fare").val(''),
        $("#Service_Fee").val(''),
        $("#Toll_Fares").val(''),
        $("#Airport_Fees").val(''),
        $("#CategoryPrice_Id").val('');
        $("#bonus_Fees").val('');
       $("#bindStateDropdown").empty();
       $("#EditButton").val("Save");
    $("#newCategoryPriceAdd").hide();
}


function OpenCategoryPriceForm(result) {

    $.ajax({
        url: "/MasterTable/GetCountry",
        type: "Get",
        success: function (data) {

            $("#bindCountryDropdown").empty().append($("<option></option>").val("0").html("Select Country"));
            $.each(data, function (data, value) {

                $("#bindCountryDropdown").append($("<option></option>").val(value.id).html(value.name));

            })

            if (result != null) {
                $("#bindCountryDropdown").val(result.state.countryId);
                $("#EditButton").val("Update");

                $.ajax({
                    url: "/MasterTable/GetState",
                    type: "POST",
                    data: {
                        Id: result.state.countryId
                    },
                    success: function (data) {


                        $("#bindStateDropdown").empty().append($("<option></option>").val("0").html("Select State"));
                        $.each(data, function (data, value) {
                            $("#bindStateDropdown").append($("<option></option>").val(value.id).html(value.name));
                        })
                        if (result.state.id != null) {
                            $("#bindStateDropdown").val(result.state.id);
                            $("#EditButton").val("Update");

                            //-------------------for get city drop down on edit ----------------------

                            $.ajax({
                                url: "/MasterTable/GetCityByStateId",
                                type: "POST",
                                data: {
                                    Id: result.state.id
                                },
                                success: function (data) {

                                    $("#bindCityDropdown").empty().append($("<option></option>").val("0").html("Select City"));
                                    $.each(data, function (data, value) {

                                        $("#bindCityDropdown").append($("<option></option>").val(value.id).html(value.name));
                                    })
                                    if (result.city.id != null) {
                                        $("#bindCityDropdown").val(result.city.id);
                                        $("#EditButton").val("Update");
                                    }
                                }
                            });
                            //--------------------------End------------


                        } else {
                            $("city_id").val('');
                            $("#EditButton").val("Save");
                        }

                    }
                });


            } else {

                $("#BaseFare").val(''),
                    $("#Cancel_Penalty").val(''),
                    $("#Cost_Per_Mile").val(''),
                    $("#Cost_Per_Minute").val(''),
                    $("#Maximum_Fare").val(''),
                    $("#Minimum_Fare").val(''),
                    $("#Sched_Cancel_Penalty").val(''),
                    $("#Sched_Ride_Minimum_Fare").val(''),
                    $("#Service_Fee").val(''),
                    $("#Toll_Fares").val(''),
                    $("#Airport_Fees").val(''),
                    $("#CategoryPrice_Id").val('');
                $("#bonus_Fees").val('');
                $("#bindStateDropdown").empty();
                $("#bindCityDropdown").empty();
                $("#EditButton").val("Save");
            }


            $("#newCategoryPriceAdd").show();

        }
    });



    $.ajax({
        url: "/MasterTable/GetRideCategory",
        type: "GET",
        success: function (data) {

            $("#bindRideCategoryDropdown").empty().append($("<option></option>").val("0").html("Select Ride Category"));
            $.each(data, function (data, value) {

                $("#bindRideCategoryDropdown").append($("<option></option>").val(value.id).html(value.name));
            })

        }
    });

}
function BindAllCategoryPriceListDatatable() {


    $.ajax({
        url: "/MasterTable/GetCategoryPriceList",
        type: "Get",
        success: function (data) {
            if (data.length > 0) {
                columnNames = Object.keys(data[0]);
                for (var i in columnNames) {
                    columns.push({
                        data: columnNames[i],
                        title: columnNames[i]
                    });
                }
            }

            $("#newCategoryPriceAdd").hide();


            $('#datatble_CategoryPriceList').DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "bDestroy": true,
                "data": data,
                "columns": [{
                    'data': 'state.country.name'
                },
                {
                    'data': 'state.name'
                },
                {
                    'data': 'city.name'
                },
                {
                    'data': 'baseFare'
                },
                {
                    'data': 'cost_Per_Mile'
                },
                {
                    'data': 'cost_Per_Minute'
                },
                {
                    'data': 'rideCategory.name'
                },
                {
                    'data': '',
                    'render': (data, type, row) => {
                        return `<a class="btn btn-warning" onclick="updateCategoryPrice(this)"  id='${row.id}' href='#'><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;&nbsp;&nbsp; <a class="btn btn-danger" onclick="deleteCategoryPrice(this)" id='${row.id}' href='#''><i class="fa-solid fa-trash"></i></a>`;
                    }
                }
                ]


            });
        }
    });

}
function Country() {
    var resultNew = {
        CountryId: $("#bindCountryDropdown").val()
    }
    var cpConError = document.getElementById('cpCountryError');
    if (resultNew.CountryId != "0") {
        cpConError.style.display = 'none';
    }
}
function State() {
    var resultNew = {
        StateId: $("#bindStateDropdown").val()
    }
    var cpStaError = document.getElementById('cpStateError');
    if (resultNew.StateId != "0") {
        cpStaError.style.display = 'none';
    }
}
function City() {
    var resultNew = {
        CityId: $("#bindCityDropdown").val()
    }
    var cpCityError = document.getElementById('cpCityError');
    if (resultNew.CityId != "0") {
        cpCityError.style.display = 'none';
    }
}
function RideCategory() {
    var resultNew = {
        RideCategoryId: $("#bindRideCategoryDropdown").val()
    }
    var cpRideCatError = document.getElementById('cpRideCategoryError');
    if (resultNew.RideCategoryId != "0") {
        cpRideCatError.style.display = 'none';
    }
}
function ValidateCategoryPrice() {

    var cpConError = document.getElementById('cpCountryError');
    cpConError.style.display = 'none';

    var cpStaError = document.getElementById('cpStateError');
    cpStaError.style.display = 'none';

    var cpCityError = document.getElementById('cpCityError');
    cpCityError.style.display = 'none';

    var cpRideCatError = document.getElementById('cpRideCategoryError');
    cpRideCatError.style.display = 'none';

    var cpBaseerror = document.getElementById('cpBaseFareError');
    cpBaseerror.style.display = 'none';

    var cpCanPanError = document.getElementById('cpCancelPaneltyError');
    cpCanPanError.style.display = 'none';

    var cpCPMilError = document.getElementById('cpCostPerMileError');
    cpCPMilError.style.display = 'none';

    var cpCPMinError = document.getElementById('cpCostPerMiniuteError');
    cpCPMinError.style.display = 'none';

    var cpMxFareError = document.getElementById('cpMaxFareError');
    cpMxFareError.style.display = 'none';

    var cpMnFareError = document.getElementById('cpMinFareError');
    cpMnFareError.style.display = 'none';

    var cpSCPanError = document.getElementById('cpSchCanPanError');
    cpSCPanError.style.display = 'none';

    var cpSMnFareError = document.getElementById('cpSchRideMinFareError');
    cpSMnFareError.style.display = 'none';

    var cpSerFeeError = document.getElementById('cpServiceFeeError');
    cpSerFeeError.style.display = 'none';

    var cpTLFareError = document.getElementById('cpTollFareError');
    cpTLFareError.style.display = 'none';

    var cpAerFeeError = document.getElementById('cpAirFeesError');
    cpAerFeeError.style.display = 'none';


    var resultNew = {
        Id: $("#CategoryPrice_Id").val(),
        BaseFare: $("#BaseFare").val(),
        Cancel_Penalty: $("#Cancel_Penalty").val(),
        Cost_Per_Mile: $("#Cost_Per_Mile").val(),
        Cost_Per_Minute: $("#Cost_Per_Minute").val(),
        Maximum_Fare: $("#Maximum_Fare").val(),
        Minimum_Fare: $("#Minimum_Fare").val(),
        Sched_Cancel_Penalty: $("#Sched_Cancel_Penalty").val(),
        Sched_Ride_Minimum_Fare: $("#Sched_Ride_Minimum_Fare").val(),
        Service_Fee: $("#Service_Fee").val(),
        Toll_Fares: $("#Toll_Fares").val(),
        Airport_Fees: $("#Airport_Fees").val(),
        RideCategoryId: $("#bindRideCategoryDropdown").val(),
        StateId: $("#bindStateDropdown").val(),
        CountryId: $("#bindCountryDropdown").val(),
        CityId: $("#bindCityDropdown").val(),
        Bonus_Amount: $("#bonus_Fees").val(),
    };
    if (resultNew.CountryId === "0") {
        cpConError.innerHTML = "Please select country*";
        cpConError.style.display = 'block';
        cpConError.style.color = "red";
        return false;
    }
    if (resultNew.StateId === "0") {
        cpStaError.innerHTML = "Please select state*";
        cpStaError.style.display = 'block';
        cpStaError.style.color = "red";
        return false;
    }
    if (resultNew.CityId === "0") {
        cpCityError.innerHTML = "Please select city*";
        cpCityError.style.display = 'block';
        cpCityError.style.color = "red";
        return false;
    }
    if (resultNew.RideCategoryId === "0") {
        cpRideCatError.innerHTML = "Please select ride category*";
        cpRideCatError.style.display = 'block';
        cpRideCatError.style.color = "red";
        return false;
    }

    if (resultNew.BaseFare.trim().length === 0) {
        cpBaseerror.innerHTML = "Please enter Base Fare*";
        cpBaseerror.style.display = 'block';
        cpBaseerror.style.color = "red";
        return false;
    }
    if (resultNew.Cancel_Penalty.trim().length === 0) {
        cpCanPanError.innerHTML = "Please enter Cancel Penality*";
        cpCanPanError.style.display = 'block';
        cpCanPanError.style.color = "red";
        return false;
    }
    if (resultNew.Cost_Per_Mile.trim().length === 0) {
        cpCPMilError.innerHTML = "Please enter Cost per Mile*";
        cpCPMilError.style.display = 'block';
        cpCPMilError.style.color = "red";
        return false;
    }


    if (resultNew.Cost_Per_Minute.trim().length === 0) {
        cpCPMinError.innerHTML = "Please enter Cost per Mintue*";
        cpCPMinError.style.display = 'block';
        cpCPMinError.style.color = "red";
        return false;
    }
    if (resultNew.Maximum_Fare.trim().length === 0) {
        cpMxFareError.innerHTML = "Please enter Maximum Fare";
        cpMxFareError.style.display = 'block';
        cpMxFareError.style.color = "red";
        return false;
    }
    if (resultNew.Minimum_Fare.trim().length === 0) {
        cpMnFareError.innerHTML = "Please enter Minimum Fare";
        cpMnFareError.style.display = 'block';
        cpMnFareError.style.color = "red";
        return false;
    }

    if (resultNew.Sched_Cancel_Penalty.trim().length === 0) {
        cpSCPanError.innerHTML = "Please enter Schedule Cancel Penalty*";
        cpSCPanError.style.display = 'block';
        cpSCPanError.style.color = "red";
        return false;
    }
    if (resultNew.Sched_Ride_Minimum_Fare.trim().length === 0) {
        cpSMnFareError.innerHTML = "Please enter Schedule Ride Minimum Fare*";
        cpSMnFareError.style.display = 'block';
        cpSMnFareError.style.color = "red";
        return false;
    }
    if (resultNew.Service_Fee.trim().length === 0) {
        cpSerFeeError.innerHTML = "Please enter Service Fee*";
        cpSerFeeError.style.display = 'block';
        cpSerFeeError.style.color = "red";
        return false;
    }
    if (resultNew.Toll_Fares.trim().length === 0) {
        cpTLFareError.innerHTML = "Please enter Toll Fares*";
        cpTLFareError.style.display = 'block';
        cpTLFareError.style.color = "red";
        return false;
    }
    if (resultNew.Airport_Fees.trim().length === 0) {
        cpAerFeeError.innerHTML = "Please enter Airport Fees*";
        cpAerFeeError.style.display = 'block';
        cpAerFeeError.style.color = "red";
        return false;
    }
    return true;
}

$('#newCategoryPriceAddForm').submit(function (e) {
    e.preventDefault();
    var maxv = checkValuesForMax();
    var minv = checkValuesForMin()
    if (!maxv)
        return false;
    if (!minv)
        return false;
    var res = ValidateCategoryPrice();
    var result = {
        Id: $("#CategoryPrice_Id").val(),
        BaseFare: $("#BaseFare").val(),
        Cancel_Penalty: $("#Cancel_Penalty").val(),
        Cost_Per_Mile: $("#Cost_Per_Mile").val(),
        Cost_Per_Minute: $("#Cost_Per_Minute").val(),
        Maximum_Fare: $("#Maximum_Fare").val(),
        Minimum_Fare: $("#Minimum_Fare").val(),
        Sched_Cancel_Penalty: $("#Sched_Cancel_Penalty").val(),
        Sched_Ride_Minimum_Fare: $("#Sched_Ride_Minimum_Fare").val(),
        Service_Fee: $("#Service_Fee").val(),
        Toll_Fares: $("#Toll_Fares").val(),
        Airport_Fees: $("#Airport_Fees").val(),
        RideCategoryId: $("#bindRideCategoryDropdown").val(),
        StateId: $("#bindStateDropdown").val(),
        CityId: $("#bindCityDropdown").val(),
        Bonus_Amount: $("#bonus_Fees").val(),
    };
    if (res && minv && maxv) {
        $.ajax({

            url: "/MasterTable/AddCategoryPrice",
            type: "POST",
            data: {
                result: result
            },
            success: function (data) {
                switch (data) {
                    case 1: swal("Category Price Added Successfully!", "", "success");
                        break;
                    case 2: swal("Category Price  Updated Successfully !", "", "success");
                        break;
                    case 3: swal("Category Price  Already Exist!", "", "warning");
                        break;
                }
                $('#conVal').text('');
                $('#staVal').text('');
                $('#rideVal').text('');
                BindAllCategoryPriceListDatatable();
            },
            error: function (jXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }

        });
    }



    return false;
});


function updateCategoryPrice(e) {

    $.ajax({
        url: "/MasterTable/GetCategoryPriceDetailById",
        type: "POST",
        data: {
            id: e.id
        },

        success: function (data) {

            OpenCategoryPriceForm(data);

            $.ajax({
                url: "/MasterTable/GetRideCategory",
                type: "GET",
                success: function (result) {

                    $("#bindRideCategoryDropdown").empty().append($("<option></option>").val("0").html("Select Ride Category"));
                    $.each(result, function (result, value) {

                        $("#bindRideCategoryDropdown").append($("<option></option>").val(value.id).html(value.name));
                        if (data.rideCategory.id != null) {
                            $("#bindRideCategoryDropdown").val(data.rideCategory.id);
                        }
                    })

                }
            });


            $("#BaseFare").val(data.baseFare),
                $("#Cancel_Penalty").val(data.cancel_Penalty),
                $("#Cost_Per_Mile").val(data.cost_Per_Mile),
                $("#Cost_Per_Minute").val(data.cost_Per_Minute),
                $("#Maximum_Fare").val(data.maximum_Fare),
                $("#Minimum_Fare").val(data.minimum_Fare),
                $("#Sched_Cancel_Penalty").val(data.sched_Cancel_Penalty),
                $("#Sched_Ride_Minimum_Fare").val(data.sched_Ride_Minimum_Fare),
                $("#Service_Fee").val(data.service_Fee),
                $("#Toll_Fares").val(data.toll_Fares),
                $("#Airport_Fees").val(data.airport_Fees),
                $("#CategoryPrice_Id").val(e.id),
                $("#bonus_Fees").val(data.bonus_Amount);

        }
    });

}


function deleteCategoryPrice(e) {
    swal({
        title: "Do you want delete this Category Price?",
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
                    url: "/MasterTable/DeleteCategoryPrice",
                    type: "POST",
                    data: {
                        id: e.id
                    },
                    success: function (data) {
                        switch (data) {
                            case 1: swal("Category Price deleted Successfully!", "", "success");
                                break;
                            case 0: swal("Opps,Category Price deletion failed!", "", "error");
                                break;
                        }
                        BindAllCategoryPriceListDatatable();

                    },
                    error: function (jXHR, textStatus, errorThrown) {
                        swal("Sorry!", "Opps, something went wrong. Please try again later.", "error");
                    }

                });
            }
        });
}