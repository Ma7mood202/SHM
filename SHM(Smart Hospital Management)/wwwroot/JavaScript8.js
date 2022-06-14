
function MyValidations() {

    var birthDate = document.getElementById('birthDate');
    var checkDate = birthDate.value.substring(0, 4);
    var currentYear = new Date().getFullYear();
    var span = document.getElementById('birthDateValidateSpan');
    if ((currentYear - checkDate) > 120) {
        span.innerHTML = "Erorr";
    }
    else if ((currentYear - checkDate) < 20) {
        span.innerHTML = "Error2";
    }



    if (bool == true) {
        var form = document.getElementById('form');
        form.submit()
    }
}