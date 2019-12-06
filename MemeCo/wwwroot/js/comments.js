/**
 * Author:    Jose Monterroso
 * Partner:   Jasen Lassig, Eli Hebdon
 * Date:      Novemeber 21, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, certify that I wrote this code from scratch and did not copy it in part or whole from
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    JavaScript file that allows the user to add, edit, and delete their comment
 */

/**
 * Ability for the user to add a new comment to a post
 * @param {any} post
 * @param {any} user
 * @param {any} e
 */
function add_comment(post, user, e) {
    e.preventDefault();

    var comment = $('#newnote').val();
    $.ajax({
        method: "POST",
        url: "../../Post/add_comment",
        data:
        {
            post_id: post,
            user_id: user,
            comment: comment
        }
    }).done(function (result) {
        if (result.success) {
            $('#add').remove();

            // Able to Edit comment 
            $('#placeHolder').html("<div class=\"card\" id=\"0\"> "
                + "<h5 class=\"card-header\">"
                + "<a href=\"/" + result.username + "\">"
                + "<img class=\"profile-pic rounded-circle\" src=" + result.profilepic + " width=\"50\" height=\"50\">  " + result.username
                + "</a> </h5>"
                + "<div class=\"card-body\">"
                + "<form onsubmit=\"edit_comment('" + result.commentid + "', event)\" action=\"edit_comment\" method=\"post\" style=\"margin-bottom: .2rem;\">"
                + "<textarea onkeydown=\"change_edit()\" id=\"note\" name=\"note\" class=\"card-text\" rows=\"4\" cols=\"73\">" + result.description + "</textarea>"
                + "<br />"
                + "<input id=\"editBtn\" class=\"btn my-2 my-sm-0 btn-sm navb\" type=\"submit\" value=\"Update\"> </form>"
                + "<button class=\"btn my-2 my-sm-0 btn-sm navb\" onclick=\"delete_comment('" + result.commentid + "', '" + 0 + "', event, '" + result.postid
                + "', '" + result.userid + "')\"> Delete</button >"
                + "</div></div> <br />");
        }
    }).fail(function (result) {
        // Server issues
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Server Error Please try again later',
            timer: 2000
        })
    });
}

/**
 * deletes the user comments
 * @param {any} comment_id
 * @param {any} comment_location
 * @param {any} e
 * @param {any} post
 * @param {any} user
 */
function delete_comment(comment_id, comment_location, e, post, user) {
    e.preventDefault();
    
    $.ajax({
        method: "POST",
        url: "../../Post/delete_comment",
        data:
        {
            comment_id: comment_id
        }
    }).done(function (result) {
        if (result.success) {
            // Remove comment
            $('#' + comment_location).remove();

            // Ability to add a commment
            $('#placeHolder').html("<div id=\"add\" class=\"card\"><h5 class=\"card-header\">Add a Comment</h5 ><div class=\"card-body\">"
                + "<form onsubmit=\"add_comment('" + post + "', '" + user + "', event)\" action=\"/Post/add_comment\"method=\"post\">"
                + "<textarea id=\"newnote\" name=\"newnote\" class=\"card-text\" rows=\"4\" cols=\"73\"></textarea>"
                + "<br />"
                + "<input class=\"btn my-2 my-sm-0 btn-sm navb\" type=\"submit\" value=\"Add\">"
                + "</form></div></div><br />");
        }
        else {
            Swal.fire({
                type: 'error',
                title: 'Oops... Server error',
                text: 'Server Error Please try again later',
                timer: 2000
            })
        }
    }).fail(function (result) {
        // Any server issues
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Server Error Please try again later',
            timer: 2000
        })
    });
    
}

/**
 * Edits the user comment
 * @param {any} comment_id
 * @param {any} e
 */
function edit_comment(comment_id, e) {
    e.preventDefault();

    var comment = $('#note').val();
    $.ajax({
        method: "POST",
        url: "../../Post/edit_comment",
        data:
        {
            comment_id: comment_id,
            comment_text: comment,
        }
    }).done(function (result) {
        // Change save button color to reflect changes
        var oldComment = result.comment_text;
        if (result.isnull) {
            $('#editBtn').css("color", "#00b0ff");
            $('#editBtn').css('border', '1px solid #00b0ff');
            $('#note').val(oldComment);
        }
        else {
            $('#editBtn').css("color", "green");
            $('#editBtn').css('border', '1px solid green');
        }
    }).fail(function (result) {
        // Any backend issues
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Server Error Please try again later',
            timer: 2000
        })
    });
}

/**
 * When the user changes the text area change color of btn
 * */
function change_edit() {
    $('#editBtn').css("color", "red");
    $('#editBtn').css('border', '1px solid red'); 
}