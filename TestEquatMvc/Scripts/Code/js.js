$(document).ready(function(){
    alert('hello');
    var gear = document.getElementById("gear");
    window.addEventListener('scroll', function () {
        gear.style.transform = "rotate("+window.pageYOffset*0.5+"deg)";
    });
});