/**
 * Author:    Jose Monterroso
 * Partner:   Jasen Lassig, Eli Hebdon
 * Date:      Novemeber 16, 2019 
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Eli, Jasen - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, certify that I wrote this code from scratch and did not copy it in part or whole from
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *   Functions that set and the get the perfered theme of the user
 *   Also searches database for users
 */

/**
 * Upon login the the users perfered theme is selected
 */
function Get_Theme() {
    var username = $('#Input_Username').val();
    var password = $('#Input_Password').val();

    $.ajax({
        method: "POST",
        url: "../../../../Home/Get_Theme",
        dataType: "json",
        data:
        {
            username: username,
            password: password,
            darkmode: false
        }
    }).done(function (result) {
        var theme = $('#ThemeToggle').attr("mode");

        if (theme == "light") {
            if (result.darkmode) { 
                // change to dark mode
                $('#ThemeToggle').attr("mode", "dark");
            }
        }
        else if (theme == "dark") {
            if (!result.darkmode) {
                // change to light
                $('#ThemeToggle').attr("mode", "light");
            }
        }
    });
}

/**
 * Sets the Theme when chosen by the user
 * @param {any} username
 */
function Set_Theme(user_id) {
    var theme = $('#ThemeToggle').attr("mode");

    $.ajax({
        method: "POST",
        url: "../../../../Home/Set_Theme",
        data:
        {
            userid: user_id,
            theme: theme
        }
    }).done({
        //Succesful found user 
    });
}

/**
 * Finds the user upon the user search 
 * */
function find_users() {
    // User to search
    var user = $('#usersearch').val();

    // Clearing dropdown
    $('#searchdropdown').html("<div id=\"searchdropdown\" class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuLink\"></div>");
    $.ajax({
        method: "POST",
        url: "../../../../Home/Find_User",
        data:
        {
            user: user
        }
    }).done(function (result) {
        // Displaying info in dropdown
        if (!result.isnull) {
            if (result.contains) {
                // Array of users
                var users = result.users;
                var pics = result.pics;
                $('#searchdropdown').html("<div id=\"searchdropdown\" class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuLink\"></div>");

                // Populating dropdown
                for (var i = 0; i < result.length; i++) {
                    $('#searchdropdown').append("<div class=\"dropdown-item\">"
                        + "<a href =\"/" + users[i] + "\">"
                        + "<img class=\"profile-pic rounded-circle\" src=\"" + pics[i] + "\" width=\"25\" height=\"25\">"
                        + "</a>"
                        + "   <a class=\"drop-link\" href=\"/" + users[i] + "\">" + users[i] + "</a>"
                        + "</div>");
                }
                return;
            }
            // No Users Found
            $('#searchdropdown').append("<a class=\"dropdown-item\" href=\"#\"> No Users Found</a>");
            return;
        }
        // Empty Search
        $('#searchdropdown').html("<div id=\"searchdropdown\" class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuLink\"></div>");
    });


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
function selectTemplate(img, templateId) {
    $('#meme-content').attr("src", img);
    var container = $('#related');
    var refreshComponent = function () {
        $.get("/Editor/GetComponent", { _templateID: templateId }, function (data) { container.html(data); });
    };
    $(function () {
        refreshComponent();
        //$("#related").load(location.href + " #related>*", "");
    })
  //  $('#related').empty();
    


 
}


function selectSize() {
    $('#meme-text').click(function () {
        $('#meme-text').css("font-size", $('#font-size-selector').val() + "px");
    })
}

function textWhite() {
    if ($('#black-text-btn').hasClass("btn-dark")) {
        $('#black-text-btn').removeClass();
        $('#black-text-btn').addClass("btn btn-outline-dark")
    }
    $('#white-text-btn').removeClass();
    $('#white-text-btn').addClass("btn btn-light")
    $('.text-block').css('color', 'white');

}

function textBlack() {
    if ($('#white-text-btn').hasClass("btn-light")) {
        $('#white-text-btn').removeClass();
        $('#white-text-btn').addClass("btn btn-outline-secondary")
    }
    $('#black-text-btn').removeClass();
    $('#black-text-btn').addClass("btn btn-dark")
    $('.text-block').css('color', 'black');
}

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

function insertText() {
    if ($('#insert-text-btn').hasClass("btn-outline-primary")) {
        $('#insert-text-btn').removeClass();
        $('#insert-text-btn').addClass("btn btn-primary")
    } else {
        $('#insert-text-btn').removeClass();
        $('#insert-text-btn').addClass("btn btn-outline-primary")
    }

}


