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
 *   functions that set and the get the perfered theme of the user
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
    }).fail({
        //Do nothing 
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

    }).fail({
        //Do Nothing
    });
}