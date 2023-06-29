$(document).ready(function () {


  // Function to open partial view
  function openPartialView1(url, containerId) {
    $("#" + containerId).empty();

    $.get(url, function (data) {
      $("#" + containerId).html(data);

      // Create a link element for the CSS file
      var cssFile = $('<link>', {
        rel: 'stylesheet',
        type: 'text/css',
        href: '/css/partialView.css' // Adjust the path to your CSS file
      });

      // Append the link element to the head of the document
      $('head').append(cssFile);
    });
  }

  //////////// Student Qualification

  // Add Qualification button click event
  $("#addQualificationButton").click(function () {
    openPartialView1("/studentj/Student/AddQualificationpv", "addQualificationPartial");
  });

  // Edit Qualification button click event
  $(".editQualificationButton").click(function () {
    var qualificationId = $(this).data("qualificationid");
    openPartialView1("/StudentJ/Student/EditQualification?qualificationId=" + qualificationId, "editQualificationPartial");
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

  // Add Referee button click event
  $("#addRefereeButton").click(function () {
    openPartialView1("/studentj/Student/AddRefereepv", "addRefereePartial");
  });

  // Edit Referee button click event
  $(".editRefereeButton").click(function () {
    var refereeId = $(this).data("refereeid");
    openPartialView1("/StudentJ/Student/EditReferee?refereeId=" + refereeId, "editRefereePartial");
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

  // Add Work Experience button click event
  $("#addWorkExperienceButton").click(function () {
    openPartialView1("/StudentJ/Student/AddWorkExperiencepv", "addWorkExperiencePartial");
  });

  // Edit Work Experience button click event
  $(".editWorkExperienceButton").click(function () {
    var workExperienceId = $(this).data("workexperienceid");
    openPartialView1("/StudentJ/Student/EditWorkExperience?workexperienceId=" + workExperienceId, "editWorkExperiencePartial");
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
