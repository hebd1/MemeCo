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