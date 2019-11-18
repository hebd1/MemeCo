function select_filter(ths, user_id, filter, e) {
    e.preventDefault();
    console.log("select filter reached");
    $.ajax({
        method: "POST",
        url: "/Home/Select_Filter",
        data:
        {
            user_id: user_id,
            filter: filter
        }
    }).done(function (result) {

        if (result.success == false) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'You have to login before you can get to memeing',
                timer: 3000
            }).then((result) => {
                location.reload();
            })
        } else {
            // set filter dropdown and reload
            $('#dropdownMenuButton').text(result.filter);
            $('#posts').load(location.href + " #posts");
           
        }

    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("fail");
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Server Error Please try again later',
            timer: 2000
        })
    }).always(function () { });

}