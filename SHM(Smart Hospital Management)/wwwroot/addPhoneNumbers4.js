
var count = 1;
var trNum = 0;
function Delete() {
    count--;

    if (trNum == 1 && count != 1) {
        var span = document.getElementById('span2');
        var am = document.getElementById('tr2');
        span.id = 'span1';
        am.id = 'tr1';
    }
    else if (trNum == 1 && count == 1) {
        var span = document.getElementById('span0');
        span.insertAdjacentHTML('afterend', '<input type="button" onclick="Add(this)" value="إضافة رقم موبايل" style="margin: 10px 0px; width: 30%; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600;" required />');
    }
    else if (trNum == 2) {
        var span = document.getElementById('span1');
        span.insertAdjacentHTML('afterend', '<input type="button" onclick="Add(this)" value="إضافة رقم موبايل" style="margin: 10px 0px; width: 30%; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600;" />');
    }
    var tr = document.getElementById("tr" + trNum);
    tr.parentNode.removeChild(tr);
    trNum--;
}
function Add(btn) {
    if (count < 3) {
        count++;
        trNum = trNum + 1;
        var attribute = document.createAttribute("style");
        attribute.value = "display:none";
        btn.setAttributeNodeNS(attribute);
        var tr = document.getElementById('tr' + (trNum - 1));
        var valueToAdd = '   <tr id="tr' + trNum + '" style=" display: flex; flex-direction: column; border-width: 1px; border-color: lightgray; border-radius: 20px; padding: 10px; margin: 10px; width: 75%; position: relative; left: 65px;">' +
            '<td> <div class="col-12 All_div"><input class="ValidateNumber" type="text" name="pn" placeholder="ادخل رقم الموبايل" /><br />' +
            '<span id="phoneValidateSpan' + trNum + '" class="text-danger"></span>'+
            '</div></td > ' +
            '<td>' +
            '<input type="button" onclick="Delete()" value="حذف" style="margin: 10px 0px; width: 15%; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600;"/> ' +
            ' </td>' +
            '<td> <div class="col-12"><span id="span' + trNum + '"></span>' +
            '<input type="button" onclick="Add(this)" value="إضافة رقم موبايل" style="margin: 10px 0px; width: 30%; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600;" /></div></td>' +
            ' </tr>';

        tr.insertAdjacentHTML('afterend', valueToAdd)
    }

}
