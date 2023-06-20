$(document).ready(function () {
    // Toggle visibility of Faculty and Department based on the selected RecruiterType
    $("#RecruiterType").on("change", function () {
        var recruiterType = $(this).val();
        if (recruiterType === "Internal") {
            $(".facultyDeptDiv").find("select").val(null);
        }
        $(".facultyDeptDiv").toggle(recruiterType === "Internal");
    }).trigger("change");


    // Toggle visibility of Part-Time Hours based on the selected JobType
    $("#JobType").on("change", function () {
        var jobType = $(this).val();
        $(".offpt").toggle(jobType === "PartTime");
    }).trigger("change");


    $(document).ready(function () {
        // Handle faculty selection change
        $('#faculty').change(function () {
            var faculty = $(this).val();
            loadDepartments(faculty);
        });
    });

 

    function loadDepartments(faculty) {
        // Make an AJAX request to fetch the departments based on the selected faculty
        $.ajax({
            url: '/jobpostA/JobPost/GetDepartments', // Replace with your controller action
            type: 'GET',
            data: { faculty: faculty },
            success: function (data) {
                // Populate the departments dropdown
                var dropdown = $('#department');
                dropdown.empty();
                dropdown.append($('<option></option>').val(''));
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').val(entry).text(entry));
                });
            }
        });
    }

    $(document).ready(function () {
        // Clear the selected option on page load
        $('#Nationality').val(null);

        // Handle change event of the select element
        $('#Nationality').change(function () {
            if ($(this).val() === '') {
                $(this).val(null);
            }
        });
    });


});
