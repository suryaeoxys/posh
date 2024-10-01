 $(document).ready(function(){
            LoadCountryData();
            $("#conTab").DataTable({
            
            });
            })
        function LoadCountryData() {
            $.ajax({
                type: "GET",
                url: '@Url.Action("GetCountries","Registration")',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(response){
                    if(response!=undefined||response!=null)
                    {
                        if(response.success==true)
                        {
                            var list=response.data;
                            console.log(list);
                            var row;
                            for(var i=0;i<list.length;i++)
                            {
                                row+=`<tr>
                                <td>${list[i].countryId}</td>
                                <td>${list[i].countryName}</td>
                                        <td><button class="btn btn-info" >Edit</button></td>
                                                <td><button class="btn btn-danger" >Delete</button></td>
                                        </tr>`;
                            }
                            $("#cbody").html(row);
                        }
                    }
                }
            });
        }