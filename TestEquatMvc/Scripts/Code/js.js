$(document).ready(function(){
    var gear = document.getElementById("gear");
    var start_elem = $('.site-welcome');
    $('.site-welcome').on('mousewheel', function (event) {
        var delta = event.originalEvent.wheelDeltaY;
        if(delta < 0)
            $(this).css('top', '-80%');
        if (delta > 0) {
            $(this).css('top', '0');
        }
    });
    $('.more').on('click', function () {
        $('.site-welcome').css('top', '-80%');
    });
    window.addEventListener('scroll', function () {
        gear.style.transform = "rotate(" + window.pageYOffset * 0.5 + "deg)";
    });
});