
function MyValidations() {

    var numberInput = document.getElementsByClassName('ValidateNumber');
    var area = document.getElementById('Area');
    var nationalNumber = document.getElementById('NationalNumber');
    var bool = true;
    for (var i = 0; i < numberInput.length; i++) {

        if (isNaN(numberInput[i].value)) {
            numberInput[i].insertAdjacentHTML('afterend','<br><span>الرجاء إدخال رقم صالح</span>')
            bool = false;
        }
    }

    if (area.value == 0) {
        area.insertAdjacentHTML('afterend', '<br><span>الرجاءاختيار منطقة</span>')
        bool = false;
    }

    if (isNaN(nationalNumber.value)) {
        nationalNumber.insertAdjacentHTML('afterend', '<br><span> الرقم الوطني يجب أن لا يحوي أحرف</span>')
        bool = false;
    }


    if (bool == true) {
        var form = document.getElementById('form');
        form.submit()
    }
}