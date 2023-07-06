

function openPartialView(url) {
    $(".partial-view-container").empty();

    $.get(url, function (data) {
        $(".partial-view-container").html(data);

        // Create a link element for the CSS file
        var cssFile = $('<link>', {
            rel: 'stylesheet',
            type: 'text/css',
            href: '/css/partialView.css' // Adjust the path to your CSS file
        });

        // Attach the onload event to ensure CSS is fully loaded before showing the partial view
        cssFile.on('load', function () {
            $(".partial-view-overlay").fadeIn();
            $(".partial-view-container").fadeIn();
            $(".partial-view-container").addClass('partial-view');
        });

        // Append the link element to the head of the document
        $('head').append(cssFile);
    });
}

//function closePartialView() {
//    $(".partial-view-container").fadeOut();
//    $(".partial-view-overlay").fadeOut();
//}