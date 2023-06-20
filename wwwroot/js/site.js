$(document).ready(function () {
    // Toggle visibility of Faculty and Department based on the selected RecruiterType
    $("#RecruiterType").on("change", function () {
        var recruiterType = $(this).val();
        $(".facultyDeptDiv").toggle(recruiterType === "Internal");
    }).trigger("change");

    // Toggle visibility of Part-Time Hours based on the selected JobType
    $("#JobType").on("change", function () {
        var jobType = $(this).val();
        $(".offpt").toggle(jobType === "PartTime");
    }).trigger("change");
});
