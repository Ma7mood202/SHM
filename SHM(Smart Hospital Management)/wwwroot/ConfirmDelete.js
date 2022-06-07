

function Confirm(i) {
    console.log(i);
    var confirm = document.getElementById('confirm'+i);
    var cancel = document.getElementById('cancel' + i);
    var btn = document.getElementById('delete' + i);
    btn.style.display = "none";
    confirm.style.display = "block";
    cancel.style.display = "block";

}
function Cancel(i) {
    console.log(i);
    var confirm = document.getElementById('confirm' + i);
    var cancel = document.getElementById('cancel' + i);
    var btn = document.getElementById('delete' + i);

    btn.style.display = "block";
    confirm.style.display = "none";
    cancel.style.display = "none";

}