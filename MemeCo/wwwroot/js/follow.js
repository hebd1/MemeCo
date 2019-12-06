/**
 * Author:    Jasen Lassig
 * Partner:   Jose Monterroso, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jasen, certify that I wrote this code from scratch and did not copy it in part or whole from
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    JavaScript file that allows the user to follow another user.
 */

/**
 * Follows the user
 * @param {any} username
 * @param {any} follow
 */
function follow(username, follow) {
    $.ajax({
        url: "Profile/Follow",
        type: 'Post',
        dataType: 'json',
        data: {
            username: username,
            follower: follow
        }
    }).done(function (result) {
        if (result.success) {
            $('#followButton').text('Following');
            $('#followButton').attr("onclick", "unfollow('" + username + "', '" + follow + "')");
            $('#followCount').text(result.followNum + " Followers");
        } else {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'You have to login before you can get to memeing',
                timer: 3000
            })
        }
    }).fail(function (result) {
        console.log("fail");
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Server Error Please try again later',
            timer: 2000
        })
    }).always(function (result) { });
}
/**
 * UnFollows the user
 * @param {any} username
 * @param {any} follow
 */
function unFollow(username, follow) {
    $.ajax({
        url: "Profile/UnFollow",
        type: 'Post',
        dataType: 'json',
        data: {
            username: username,
            follower: follow
        }
    }).done(function (result) {
        if (result.success) {
            $('#followButton').text('Follow');
            $('#followButton').attr("onclick", "unfollow('" + username + "', '" + follow + "')");
            $('#followCount').text(result.followNum + " Followers");
        } else {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'You have to login before you can get to memeing',
                timer: 3000
            })
        }
    }).fail(function (result) {
        console.log("fail");
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Server Error Please try again later',
            timer: 2000
        })
    }).always(function (result) { });
}