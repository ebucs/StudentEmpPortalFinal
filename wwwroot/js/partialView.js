$(document).ready(function () {
    // Search input event
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

    // Function to open partial view
    function openPartialView(url, containerId) {
        $("#" + containerId).empty();

        $.get(url, function (data) {
            $("#" + containerId).html(data);

            // Create a link element for the CSS file
            var cssFile = $('<link>', {
                rel: 'stylesheet',
                type: 'text/css',
                href: '/css/partialView.css' // Adjust the path to your CSS file
            });

            // Attach the onload event to ensure CSS is fully loaded before showing the partial view
            cssFile.on('load', function () {
                $(".partial-view-overlay").fadeIn();
                $("#" + containerId).fadeIn();
                $("#" + containerId).addClass('partial-view');
            });

            // Append the link element to the head of the document
            $('head').append(cssFile);
        });
    }

    // Function to close partial view
    function closePartialView() {
        $(".partial-view-container").fadeOut();
        $(".partial-view-overlay").fadeOut();
    }

    //////////// Student Qualification

    // Add Qualification button click event
    $("#addQualificationButton").click(function () {
        openPartialView("/studentj/Student/AddQualificationpv", "addQualificationPartial");
    });

    // Edit Qualification button click event
    $(".editQualificationButton").click(function () {
        var qualificationId = $(this).data("qualificationid");
        openPartialView("/StudentJ/Student/EditQualification?qualificationId=" + qualificationId, "editQualificationPartial");
    });

    // Delete Qualification button click event
    $(".deleteQualificationButton").click(function () {
        var qualificationId = $(this).data("qualificationid");
        if (confirm("Are you sure you want to delete this qualification?")) {
            $.ajax({
                url: "/StudentJ/Student/DeleteQualification",
                type: "POST",
                data: { qualificationId: qualificationId },
                success: function () {
                    location.reload();
                }
            });
        }
    });


    //////////// Student Referee

    //Add Referee button click event
    $("#addRefereeButton").click(function () {
        openPartialView("/studentj/Student/AddRefereepv", "addRefereePartial");
    });

    // Edit Referee button click event
    $(".editRefereeButton").click(function () {
        var refereeId = $(this).data("refereeid");
        openPartialView("/StudentJ/Student/EditReferee?refereeId=" + refereeId, "editRefereePartial");
    });

    // Delete Referee button click event
    $(".deleteRefereeButton").click(function () {
        var refereeId = $(this).data("refereeid");
        if (confirm("Are you sure you want to delete this Referee?")) {
            $.ajax({
                url: "/StudentJ/Student/DeleteReferee",
                type: "POST",
                data: { refereeId: refereeId },
                success: function () {
                    location.reload();
                }
            });
        }
    });


    //////////// Student Work Experience

    // Add WorkExperience
    $("#addWorkExperienceButton").click(function () {
        openPartialView("/StudentJ/Student/AddWorkExperiencepv", "addWorkExperiencePartial");
    });

    // Edit Qualification button click event
    $(".editWorkExperienceButton").click(function () {
        var workExperienceId = $(this).data("workexperienceid");
        openPartialView("/StudentJ/Student/EditWorkExperience?workexperienceId=" + workExperienceId, "editWorkExperiencePartial");
    });

    // Delete Work Experience button click event
    $(".deleteWorkExperienceButton").click(function () {
        var workExperienceId = $(this).data("workexperienceid");
        if (confirm("Are you sure you want to delete this Work Experience?")) {
            $.ajax({
                url: "/StudentJ/Student/DeleteWorkExperience",
                type: "POST",
                data: { workExperienceId: workExperienceId },
                success: function () {
                    location.reload();
                }
            });
        }
    });



});
