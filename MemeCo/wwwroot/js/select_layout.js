function select_layout(button) {

    // set image layout
    if (button == "image-bottom-btn") {
        $('#image').html("<textarea id=\"meme-text\"  onclick=\"selectSize()\"></textarea>");
        $('#image').append("<img id=\"meme-content\" class=\"img-fluid\" alt=\"Demo Image\" src=\"/files/img_placeholder.png\">");
    } else if (button == "image-top-btn") {
        $('#image').html("<img id=\"meme-content\" class=\"img-fluid\" alt=\"Demo Image\" src=\"/files/img_placeholder.png\">");
        $('#image').append("<textarea id=\"meme-text\"  onclick=\"selectSize()\"></textarea>");
    } else if (button == "single-image-btn") {
        $('#image').html("");
        $('#image').append("<img id=\"meme-content\" class=\"img-fluid\" alt=\"Demo Image\" src=\"/files/img_placeholder.png\">");
    } // TODO implement 2x2 grid
   

    // set active button
    if ($('#single-image-btn').hasClass("btn-primary")){

        $('#single-image-btn').removeClass("btn-primary");
        $('#single-image-btn').addClass("btn btn-outline-primary");

    } else if ($('#image-top-btn').hasClass("btn-primary")) {

        $('#image-top-btn').removeClass("btn-primary");
        $('#image-top-btn').addClass("btn btn-outline-primary");

    } else if ($('#image-bottom-btn').hasClass("btn-primary")) {

        $('#image-bottom-btn').removeClass("btn-primary");
        $('#image-bottom-btn').addClass("btn btn-outline-primary");

    } else if ($('#2x2-grid-btn').hasClass("btn-primary")) {

        $('#2x2-grid-btn').removeClass("btn-primary");
        $('#2x2-grid-btn').addClass("btn btn-outline-primary");

    }
    $('#' + button).removeClass();
    $('#' + button).addClass("btn btn-primary");
}