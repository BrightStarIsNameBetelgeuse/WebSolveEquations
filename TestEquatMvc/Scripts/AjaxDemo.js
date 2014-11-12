$(document).ready(function () {
    $(".cramerMethod").click(function (evt) {
        var cell = $(evt.target).closest("div").data;
        var custID = cell.text();
        $("#viewPlaceHolder").load("/home",
            { customerID: custID });
        $('#result').ht
    });

    $(".gaussMethod").click(function (evt) {

    });
});