//Enable Tooltips
$(document).ready(function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
    var toastElList = [].slice.call(document.querySelectorAll('.toast'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl, {
            autohide: true
        }).show()
    })
});

//Checkbox code for the checkbox on home.
//Check on pageload
checkboxStatus();
//Check on change
$('#editPlan').change(checkboxStatus);

function checkboxStatus() {
    if ($('#editPlan').is(':checked') == true) {
        $('.actions').show();
    } else {
        $('.actions').hide();
    }
}
