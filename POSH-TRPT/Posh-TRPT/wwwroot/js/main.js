  $(function () {
    $('.datatable ').DataTable({
      "paging": true,
      "lengthChange": false,
      "searching": false,
      "ordering": true,
      "info": true,
        "autoWidth": false,
      "responsive": true,
    });

    function checkmobile() {
      if ($(window).width() <= 600) {
        return true;
      } else {
        return false;
      }
    }

    function checkipad() {
      if ($(window).width() <= 768) {
        return true;
      } else {
        return false;
      }
    }

    function checkipadpro() {
      if ($(window).width() <= 1024) {
        return true;
      } else {
        return false;
      }
    }
    $('.sidebar-toggler').click(function () {
      if (checkipad()) {
        $('#left-sidebar').toggleClass('show');
      } else if (checkipadpro()) {
        $('#left-sidebar').toggleClass('width-250');
      } else {
        $('#left-sidebar').toggleClass('sidebar-collapse');
        $('#right-sidebar').toggleClass('full-right-sidebar');
      }

    })


  });

  function activeMenu(menu) {
    var borderclass = "";
    if (menu == "activeuser" || menu == "totalrevenue") {
      borderclass = "border-green";
    } else if (menu == "inactiveuser") {
      borderclass = "border-navy";
    } else if (menu == "unverified") {
      borderclass = "border-orange";
    } else if (menu == "mastertab") {
        borderclass = "border-purple";
    } else if (menu == "country") {
      borderclass = "border-purple";
    }else if (menu == "state") {
        borderclass = "border-purple";
    }else if (menu == "city") {
        borderclass = "border-purple";
    }else {
      borderclass = "";
    }
    if (menu == "totalrevenue") {

      if ($('#nav-tab li[href="#alluser"]').addClass('show')) {

        $("div[data-id='allusertab']").addClass('show  active');
      }
      $('#alluser').addClass('show active');
    } else {
      $('#alluser').removeClass('show active');
    }

    $('#left-sidebar .nav-link, #nav-tab .nav-link').removeClass('active');
    $(`#nav-tabContent .tab-pane`).removeClass('show active');
    $(`.${menu}`).addClass('active');
    $(`#nav-tab li[href="#${menu}"]`).addClass('active');

    $("div[data-type='analyticsCard']").removeClass();
    $("div[data-type='analyticsCard']").addClass('card analytics-card');

    $("div[data-id='" + menu + "']").addClass(borderclass);

    $(`#${menu}`).addClass('show active');
  }