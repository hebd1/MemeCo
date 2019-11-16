// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


/**
 * Upon login the the users perfered theme is selected
 */
function Get_Theme() {
    console.log("GET THEME");
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
    console.log("Set_Theme");
    console.log(user_id)

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