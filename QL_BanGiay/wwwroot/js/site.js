// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            const myModal = new bootstrap.Modal(document.getElementById('form-modal'));
            myModal.show();
        },
        error: function (err) {
            console.log(err)
        }
    })
}
//$(document).ready(function () {
//    getNhanHieu();
//    getNoiSanXuat();
//    $("#imgPreview").hide();

//    let id = $('#NhanHieu').val();
//    if (id == "")
//        $('#MaBoSuuTap').attr('disabled', true);
//    else
//        $('#MaBoSuuTap').attr('disabled', false);
//    let id_bst = $('#MaBoSuuTap').val();
//    $.ajax({
//        url: '/Admin/Giay/BoSuuTap1?id=' + id + '&&key=' + id_bst,
//        success: function (result) {
//            $.each(result, function (i, data) {
//                $('#MaBoSuuTap').append('<option value=' + data['maBoSuuTap'] + '>' + data['tenBoSuuTap'] + '</option>');
//            });
//        }
//    });
//    $("#NhanHieu").change(function () {
//        var id = $(this).val();
//        $('#MaBoSuuTap').attr('disabled', false);
//        $('#MaBoSuuTap').empty();
//        $('#MaBoSuuTap').append('<option hidden value="">----Chọn----</option>');
//        $.ajax({
//            url: '/Admin/Giay/BoSuuTap?id=' + id,
//            success: function (result) {
//                $.each(result, function (i, data) {
//                    $('#MaBoSuuTap').append('<option value=' + data['maBoSuuTap'] + '>' + data['tenBoSuuTap'] + '</option>');
//                });
//            }
//        });
//    });
//    //Preview Images
//    $("#imageBrowes").change(function () {

//        var File = this.files;

//        if (File && File[0]) {
//            ReadImage(File[0]);
//        }

//    });
//});
//preview images
var ReadImage = function (file) {

    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {

        image.src = _file.target.result;
        image.onload = function () {
            $("#targetImg").attr('src', _file.target.result);
            $("#imgPreview").show();
        }

    }

}
//Preview images array
function preview_image() {
    var total_file = document.getElementById("imagesBrowes").files.length;
    for (var i = 0; i < total_file; i++) {
        $('#image_preview').append("<img style='width: 60px; height: 60px;' src='" + URL.createObjectURL(event.target.files[i]) + "'><br>");
    }
}

function getNhanHieu() {
    let id = $('#NhanHieu').val();
    $.ajax({
        url: '/Admin/Giay/NhanHieu?id=' + id,
        success: function (result) {
            $.each(result, function (i, data) {
                $('#NhanHieu').append('<option value=' + data['maNhanHieu'] + '>' + data['tenNhanHieu'] + '</option>');
            });
        }
    });
}
function getNoiSanXuat() {
    let id = $('#MaNhaSanXuat').val();
    $.ajax({
        url: '/Admin/Giay/NoiSanXuat?id=' + id,
        success: function (result) {
            $.each(result, function (i, data) {
                $('#MaNhaSanXuat').append('<option value=' + data['maNhaSanXuat'] + '>' + data['tenNhaSanXuat'] + '</option>');
            });
        }
    })
}
jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    console.log(res['html']);
                    $('#view-all').html(res.html);
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $("#form-modal .close").click();
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
jQueryAjaxDelete = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    console.log(res['html']);
                    $('#view-all').html(res.html);
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $("#form-modal .close").click();
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//Create Hóa đơn nhập hàng
function AddItem(btn) {

    var table;
    table = document.getElementById('CodesTable');
    var rows = table.getElementsByTagName('tr');
    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastrowIdx = rows.length - 2;

    var nextrowIdx = eval(lastrowIdx) + 1;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);

    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;

    var x = document.getElementsByTagName("INPUT");

    for (var cnt = 0; cnt < x.length; cnt++) {
        if (x[cnt].type == "text" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0) {
            if (x[cnt].id.indexOf('Unit') == 0)
                x[cnt].value = '';
        }
        else if (x[cnt].type == "number" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
            x[cnt].value = 0;
    }

    rebindvalidators();
}
function rebindvalidators() {


    var $form = $("#CodeSbyAnizForm");

    $form.unbind();

    $form.data("validator", null);

    $.validator.unobtrusive.parse($form);

    $form.validate($form.data("unobtrusiveValidation").options);

}
function DeleteItem(btn) {

    var table = document.getElementById('CodesTable');
    var rows = table.getElementsByTagName('tr');

    var btnIdx = btn.id.replaceAll('btnremove-', '');
    var idOfQuantity = btnIdx + "__SoLuong";
    console.log(idOfQuantity);
    var txtQuantity = document.querySelector("[id$='" + idOfQuantity + "']");

    txtQuantity.value = 0;


    var idOfIsDeleted = btnIdx + "__IsDeleted";
    var txtIsDeleted = document.querySelector("[id$='" + idOfIsDeleted + "']");
    txtIsDeleted.value = "true";
    $(btn).closest('tr').hide();

    CalcTotals();

}
function setSameWidth(srcElement, desElement) {
    desElement.style.width = "230px";
}
function CalcTotals() {
    var x = document.getElementsByClassName('QtyTotal');
    var totalQty = 0;
    var Amount = 0;
    var totalAmount = 0;
    var i;
    for (i = 0; i < x.length; i++) {

        var idofIsDeleted = i + "__IsDeleted";
        var idofPrice = i + "__GiaMua";

        var idofTotal = i + "__Total";

        var hidIsDelId = document.querySelector("[id$='" + idofIsDeleted + "']").id;

        var priceTxtId = document.querySelector("[id$='" + idofPrice + "']").id;

        var totalTxtId = document.querySelector("[id$='" + idofTotal + "']").id;

        if (document.getElementById(hidIsDelId).value != "true") {
            totalQty = totalQty + eval(x[i].value);

            var txttotal = document.getElementById(totalTxtId);
            var txtprice = document.getElementById(priceTxtId);

            txttotal.value = eval(x[i].value) * txtprice.value;

            totalAmount = eval(totalAmount) + eval(txttotal.value);
        }
    }

    document.getElementById('txtQtyTotal').value = new Intl.NumberFormat('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(totalQty);
    document.getElementById('txtAmountTotal').value = new Intl.NumberFormat('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(totalAmount);

    return;
}
document.addEventListener('change', function (e) {
    if (e.target.classList.contains('QtyTotal')
        || e.target.classList.contains('PriceTotal')
    ) {
        CalcTotals();
    }
}
    , false);
function ShowSearchableList(event) {
    if (event.target.id.indexOf('MaGiay') < 0) {
        return;
    }
    var tid = event.target.id;
    var txtDescId = tid.replaceAll('MaGiay', 'Description');
    var txtValue = document.getElementById('txtValue');
    var txtText = document.getElementById(txtDescId);
    var txtSearch = event.target;
    var lstbox = document.getElementById("mySelect");
    $(txtSearch).after($(lstbox).show('slow'));
    if (event.keyCode === 13 || event.keyCode == 9) {
        txtSearch.value = txtValue.value;
        lstbox.style.visibility = "hidden";
        var divlst = document.getElementById("HiddenDiv");
        $(divlst).after($(lstbox).show('slow'));
        if (event.keyCode === 13) {
            event.preventDefault();
            txtSearch.focus();
            return;
        }
        else
            return;
    }
    setSameWidth(txtSearch, lstbox);
    lstbox.style.visibility = "visible";
    txtValue.value = "";
    txtText.value = "";
    var items = lstbox.options;
    for (var i = items.length - 1; i >= 0; i--) {
        if (items[i].text.toLowerCase().indexOf(txtSearch.value.toLowerCase()) > -1) {
            items[i].style.display = 'block';
            items[i].selected = true;
            txtValue.value = items[i].value;
            txtText.value = items[i].text;
        }
        else {
            items[i].style.display = 'none';
            items[i].selected = false;
        }
    }
    var objDiv = document.getElementById("CsDiv");
    objDiv.scrollTop = objDiv.scrollHeight - 200;
}
document.addEventListener('keydown', ShowSearchableList);