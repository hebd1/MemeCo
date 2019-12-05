/**
 * Author:    Eli Hebdon
 * Partner:   Jasen Lassig, Jose Monterroso
 * Date:      Novemeber 21, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, certify that I wrote this code from scratch and did not copy it in part or whole from
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    JavaScript file that allows handles meme editor functions
 */

/**
 * Creates ajax request to add the current meme to the database
 * */
function post_meme(ths, user_id, template_id, e) {
    e.preventDefault();
    console.log("post meme reached");

    html2canvas(document.getElementById('image')).then(function (canvas) {
        var imageData = canvas.toDataURL("image/png");

        var caption = $('#caption-text').val();

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

                $('#previewModal').modal('toggle');

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

/**
 * Sets the layout of the editor
 * */
function select_layout(button) {
    //<div id="image" class="justify-content-center">
    //    <img id="meme-content" alt="Demo Image" class="img-fluid meme-class" src="~/files/img_placeholder.png">
    //            </div>
    // set image layout
    if (button == "image-bottom-btn") {
        $('#image').html("<img id=\"meme-text\" class=\"img-fluid meme-class\"  src=\"/files/textbox.PNG\"></img>");
        $('#image').append("<img id=\"meme-content\" class=\"img-fluid meme-class\" alt=\"Demo Image\" src=\"/files/img_placeholder.png\">");
    } else if (button == "image-top-btn") {
        $('#image').html("<img id=\"meme-content\" class=\"img-fluid meme-class\" alt=\"Demo Image\" src=\"/files/img_placeholder.png\">");
        $('#image').append("<img id=\"meme-text\" class=\"img-fluid meme-class\" src=\"/files/textbox.PNG\"></img>");
    } else if (button == "single-image-btn") {
        $('#image').html("");
        $('#image').append("<img id=\"meme-content\" class=\"img-fluid meme-class\" alt=\"Demo Image\" src=\"/files/img_placeholder.png\">");
    } // TODO implement 2x2 grid

}

/**
 * displays a previous of the edited meme in a modal in the editor view
 * */
function showPreview() {
    var modal = $('#previewModal');
    modal.find('canvas').remove();

    html2canvas(document.getElementById('image')).then(function (canvas) {
        modal.find('.modal-body').append(canvas);
        console.log(canvas);
    });
    $('#previewModal').modal();

}

/**
 * Sets the image in the editor to the user selected template
 * */
function selectTemplate(img, templateId, userId) {
    $('#meme-content').attr("src", img);
    var postButton = $('#post-btn');
    postButton.replaceWith('<button id="post-btn" type="button" class="btn btn-primary" onclick="post_meme(this, \'' + userId + '\', \'' + templateId + '\', event)">Post to MemeCo</button>');
    var container = $('#related');
    var refreshComponent = function () {
        $.get("/Editor/GetComponent", { _templateID: templateId }, function (data) { container.html(data); });
    };
    $(function () {
        refreshComponent();
    })
}


/**
 * Sets the font size of inserted text in the editor
 * */
function selectSize() {
    $('#meme-text').click(function () {
        $('#meme-text').css("font-size", $('#font-size-selector').val() + "px");
    })
}


/**
 * Sets the text-color to white
 * */
function textWhite() {
    if ($('#black-text-btn').hasClass("btn-dark")) {
        $('#black-text-btn').removeClass();
        $('#black-text-btn').addClass("btn btn-outline-dark")
    }
    $('#white-text-btn').removeClass();
    $('#white-text-btn').addClass("btn btn-light")
    $('.text-block').css('color', 'white');

}

/**
 * Sets the text-color to black
 * */
function textBlack() {
    if ($('#white-text-btn').hasClass("btn-light")) {
        $('#white-text-btn').removeClass();
        $('#white-text-btn').addClass("btn btn-outline-secondary")
    }
    $('#black-text-btn').removeClass();
    $('#black-text-btn').addClass("btn btn-dark")
    $('.text-block').css('color', 'black');
}

/**
 * Downloads the editor canvas to the user's desktop
 * */
function download() {
    html2canvas(document.getElementById('image')).then(function (canvas) {
        var imgageData =
            canvas.toDataURL("image/png");
        var newData = imgageData.replace(
            /^data:image\/png/, "data:application/octet-stream");

        $("#btn-Convert-Html2Image").attr(
            "download", "meme.png").attr(
                "href", newData);
    });

}

/**
 * Enables text to be inserted on the canvas
 * */
function insertText() {
    if ($('#insert-text-btn').hasClass("btn-outline-primary")) {
        $('#insert-text-btn').removeClass();
        $('#insert-text-btn').addClass("btn btn-primary")
    } else {
        $('#insert-text-btn').removeClass();
        $('#insert-text-btn').addClass("btn btn-outline-primary")
    }

}
