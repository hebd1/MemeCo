function post_meme(ths, user_id, template_id, e) {
    e.preventDefault();
    console.log("post meme reached");
    var meme = $('#meme-content').attr('src');
   
    var caption = $('#caption-text').text();
    $.ajax({

        method: "POST",
        url: "/Editor/Post_Meme",
        data:
        {
            user_id: user_id,
            description: caption,
            image: meme,
            template_id: template_id
        }
    }).done(function (result) {

        if (result.success == false) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'You have to login before you can get to memeing',
                timer: 3000
            })
        } else {

            
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