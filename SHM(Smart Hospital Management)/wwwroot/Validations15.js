
function MyValidations() {

    var numberInput = document.getElementsByClassName('ValidateNumber');
    var area = document.getElementById('Area');
    var nationalNumber = document.getElementById('NationalNumber');
    var bool = true;
    var birthDate = document.getElementById('birthDate');
    var checkDate = birthDate != null ? birthDate.value.substring(0, 4):0;
    var birthDateForPatient = document.getElementById('birthDateForXY');
    var checkDateForPatient = birthDateForPatient != null? birthDateForPatient.value.substring(0, 4):0;
    var currentYear = new Date().getFullYear();
    var XY = document.getElementById('XY');
    var span = document.getElementById('birthDateValidateSpan');
    var spanForPatient = document.getElementById('birthDateValidateSpanForXY');
    var spanForXY = document.getElementById('ValidateXY');
    console.log("omar");
    if (checkDateForPatient != 0) {
        if (XY.value == '')
            spanForXY.innerHTML = "الرجاء تحديد موقع منزل المريض";
        else
            spanForXY.innerHTML = '';
    }
    console.log("omar1");
    if ((currentYear - checkDate) > 120 && checkDate != 0) {
        span.innerHTML = "لا يمكن ان يكون العمر اكبر من 120 عام";
    }
    else if ((currentYear - checkDate) < 20 && checkDate!=0) {
        span.innerHTML = "يجب ان يكون العمر 20 عام على الأقل";
    }
    else if (checkDate!=0) {
        span.innerHTML = '';
    }
    if ((currentYear - checkDateForPatient) > 120 && checkDateForPatient!=0) {
        spanForPatient.innerHTML = "لا يمكن ان يكون العمر اكبر من 120 عام";
    }
    else if (checkDateForPatient!=0) {
        spanForPatient.innerHTML = "";
    }
    for (var i = 0; i < numberInput.length; i++) {

      
        if (numberInput[i].value == '') {
            document.getElementById('phoneValidateSpan' + i).innerHTML = 'الرجاء إدخال رقم';
            bool = false;
         }
         else if (isNaN(numberInput[i].value) || numberInput[i].value.length < 10) {
             document.getElementById('phoneValidateSpan' + i).innerHTML = 'الرجاء إدخال رقم صالح';
             bool = false;
         }
        else {
            document.getElementById('phoneValidateSpan' + i).innerHTML = "";
        }
    }

    if (area.value == 0 || area.value == 'اختر المنطقة') {
        document.getElementById('areaValidateSpan').innerHTML = 'الرجاء اختيار منطقة';
        bool = false;
    }
    else {
        document.getElementById('areaValidateSpan').innerHTML = '';
    }

    if (isNaN(nationalNumber.value)) {
        document.getElementById('nationalValidateSpan').innerHTML = 'الرقم الوطني يجب أن لا يحوي أحرف';
        bool = false;
    }
    else {
        document.getElementById('nationalValidateSpan').innerHTML = "";
    }


    if (bool == true) {
        var form = document.getElementById('form');
        form.submit()
    }
}