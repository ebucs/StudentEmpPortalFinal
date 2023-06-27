$(document).ready(function () {

    // Toggle visibility of Part-Time Hours based on the selected JobType
    $("#JobType").on("change", function () {
        var jobType = $(this).val();
        $(".offpt").toggle(jobType === "PartTime");
    }).trigger("change");

 
    // Handle faculty selection change
    $('#faculty').change(function () {
        var faculty = $(this).val();
        loadDepartments(faculty);
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
                dropdown.append($('<option>Select a Department</option>').val(''));
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').val(entry).text(entry));
                });
            }
        });
    }

    // Handle faculty selection change on the student side
    $('#facultyd').change(function () {
        var facultyd = $(this).val();
        loadStudentDepartments(facultyd);
    });

    function loadStudentDepartments(facultyd) {
        // Make an AJAX request to fetch the departments based on the selected faculty
        $.ajax({
            url: '/studentj/Student/GetDepartments', // Replace with your controller action
            type: 'GET',
            data: { facultyd: facultyd },
            success: function (data) {
                // Populate the departments dropdown
                var dropdown = $('#studentDepartment'); // Update the ID here
                dropdown.empty();
                dropdown.append($('<option>Select a Department</option>').val(''));
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').val(entry).text(entry));
                });
            }
        });
    }



});
