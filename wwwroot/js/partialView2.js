$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var searchText = $(this).val().toLowerCase();
        $('tbody tr').each(function () {
            var firstName = $(this).find('td:first-child').text().toLowerCase();
            var registeredName = $(this).find('td:nth-child(2)').text().toLowerCase();
            var tradingName = $(this).find('td:nth-child(3)').text().toLowerCase();
            var regNumber = $(this).find('td:nth-child(4)').text().toLowerCase();
            var status = $(this).find('td:nth-child(6)').text().toLowerCase();

            if (
                firstName.includes(searchText) ||
                registeredName.includes(searchText) ||
                tradingName.includes(searchText) ||
                regNumber.includes(searchText) ||
                status.includes(searchText)
            ) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});

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

function closePartialView() {
    $(".partial-view-container").fadeOut();
    $(".partial-view-overlay").fadeOut();
}