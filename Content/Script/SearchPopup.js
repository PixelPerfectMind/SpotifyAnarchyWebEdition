/*

    == Summary ===========================================

    This script is used to display the search popup when
    the user clicks on the search icon in the header.

    ======================================================
*/

// On sroll down, add 'navbar-scrolled' class to navbar
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 1) {
        document.getElementById("navbar").classList.add("navbar-scrolled");
    } else {
        document.getElementById("navbar").classList.remove("navbar-scrolled");
    }
}

function OpenCloseCurtain(action) {

    var window = document.getElementById("searchWindow");
    var curtain = document.getElementById("curtain");

    if (action == "open") {
        curtain.style.opacity = "0";
        window.style.scale = "0";

        setTimeout(function () { curtain.style.opacity = "1"; window.style.scale = "1"; }, 24);
        document.getElementById("curtain").style.display = "flex";

        document.getElementById("body").classList.add("blocked");

    }
    else if (action == "close") {
        curtain.style.opacity = "0";
        window.style.scale = "0";

        setTimeout(function () { curtain.style.display = "none"; }, 200);
        document.getElementById("body").classList.remove("blocked");
    }
}

function RedirectToSearchView() {
    var query = document.getElementById('searchQuery').value;
    query = query.replace(/ /g, "+");

    let types = ["track", "album", "artist", "playlist", "show", "episode", "audiobook"].toString();
    types = types.replace(/,/g, "%2C");

    window.location.href = "/search?query=" + query + "&type=" + types + "&market=DE";
}