function post_meme(ths, user_id, template_id, e) {
    e.preventDefault();
    console.log("post meme reached");

    html2canvas(document.getElementById('image')).then(function (canvas) {
        var imageData = canvas.toDataURL("image/png");

        var caption = $('#caption-text').text();

        $.ajax({

            method: "POST",
            url: "/Editor/Post_Meme",
            data:
            {
                user_id: user_id,
                description: caption,
                image: imageData,
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
                Swal.fire({
                    title: 'Meme Posted!',
                    showClass: {
                        popup: 'animated fadeInDown faster'
                    },
                    hideClass: {
                        popup: 'animated fadeOutUp faster'
                    }
                })

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


    });
   
   
}