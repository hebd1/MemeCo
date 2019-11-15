function handle_change(ths, user_id, liked, post_id, e) {
    e.preventDefault();
    console.log("handle change reached");
    $.ajax({
        method: "POST",
        url: "/Home/Like_Post",
        data:
        {
            user_id: user_id,
            liked: liked,
            post_id: post_id
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
            location.reload();
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