$(document).ready(function () {

    getTinh();
    let toggleBtn = document.getElementById('toggleBtn');
    toggleBtn.onclick = function () {
        if (pswrd.type == 'password') {
            pswrd.setAttribute('type', 'text');
            toggleBtn.classList.add('hide');
        } else {
            pswrd.setAttribute('type', 'password');
            toggleBtn.classList.remove('hide');
        }

    }
    let toggleBtnCf = document.getElementById('toggleBtnCf');
    toggleBtnCf.onclick = function () {
        if (pwcf.type == 'password') {
            pwcf.setAttribute('type', 'text');
            toggleBtnCf.classList.add('hide');
        } else {
            pwcf.setAttribute('type', 'password');
            toggleBtnCf.classList.remove('hide');
        }
    }
    $("#validation").hide();
    $('#Huyen').attr('disabled', true);
    $('#Xa').attr('disabled', true);
    $("#Tinh").change(function () {
        var id = $(this).val();
        $('#Huyen').attr('disabled', false);
        $('#Huyen').empty();
        $('#Huyen').append('<option value="">----Chọn----</option>');
        $('#Xa').attr('disabled', true);
        $('#Xa').empty();
        $('#Xa').append('<option value="">----Chọn----</option>');
        $.ajax({
            url: '/account/Huyen?id=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#Huyen').append('<option value=' + data['maHuyen'] + '>' + data['tenHuyen'] + '</option>');
                });
            }
        });
    });
    $("#Huyen").change(function () {
        $('#Xa').attr('disabled', false);
        var id = $(this).val();
        $('#Xa').empty();
        $('#Xa').append('<option value="">----Chọn----</option>');
        $.ajax({
            url: '/account/Xa?id=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#Xa').append('<option value=' + data['maXa'] + '>' + data['tenXa'] + '</option>');
                });
            }
        });
    });
});
function getTinh() {
    $.ajax({
        url: '/account/Tinh',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#Tinh').append('<option value=' + data['maTinh'] + '>' + data['tenTinh'] + '</option>');
            });
        }
    });
}