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
            post_id: post_id,
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

            var likedButton = $('#' + result.post_id + '-like');
            var dislikedButton = $('#' + result.post_id + '-dislike');
            if (result.liked == true) {

                if (likedButton.attr('class') == "fas fa-thumbs-up") {
                    // undo like
                    likedButton.removeClass().addClass("far fa-thumbs-up");
                }
                // set like
                else if (likedButton.attr('class') == "far fa-thumbs-up") {
                    likedButton.removeClass().addClass("fas fa-thumbs-up");
                    dislikedButton.removeClass().addClass("far fa-thumbs-down");
                }
            } else if (result.liked == false) {
                if(dislikedButton.attr('class') == "fas fa-thumbs-down") {
                    // undo dislike
                    dislikedButton.removeClass().addClass("far fa-thumbs-down");
                }
                // set dislike
                else if (dislikedButton.attr('class') == "far fa-thumbs-down") {
                    dislikedButton.removeClass().addClass("fas fa-thumbs-down");
                    likedButton.removeClass().addClass("far fa-thumbs-up");
                }
            }
   
            // update like/dislike ratio in progress bar
            var likeProgress = $('#' + result.post_id + '-like-percent');
            likeProgress.css('width', result.like_percent);
            likeProgress.text(result.like_percent);
            var dislikeProgress = $('#' + result.post_id + '-dislike-percent');
            dislikeProgress.css('width', result.dislike_percent);
            dislikeProgress.text(result.dislike_percent);
            //location.reload();
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