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
        data:
        {
            username: username,
            password: password,
            DarkMode: false
        }
    }).done(function (result) {
        var theme = $('#ThemeToggle').attr("mode");
        console.log(theme);

        if (theme == "light") {
            console.log("in the light theme if");
            if (result.DarkMode) {
                console.log("in the result.dark mode if");
                // change to dark mode
                $('#ThemeToggle').click();
                console.log("clicked theme toggle");
            }
        }
        else if (theme.toString() == "dark") {
            if (result.DarkMode == false) {
                // change to light
                $('#ThemeToggle').click();
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


    // e.preventDefault();

 

    console.log("get_theme");
    console.log(password);
    console.log(username);

    $.ajax({

        //method: "POST",
        //url: "/Home/Get_Theme",
        //  data:
        // {
        //     username: username
        // }
    }).done({
        //Succesful found user 

    }).fail({

    }).always({});
   
}