// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#MaDongSanPham').append('<option hidden value="">----Chọn dòng sản phẩm----</option>');
$("#MaNhanHieu").change(function () {
    var id = $(this).val();
    $('#MaDongSanPham').empty();
    $('#MaDongSanPham').append('<option hidden value="">----Chọn dòng sản phẩm----</option>');
    $.ajax({
        url: '/admin/shoe/GetCollections?id=' + id,
        success: function (result) {
            $.each(result, function (i, data) {
                $('#MaDongSanPham').append('<option value=' + data['maDongSanPham'] + '>' + data['tenDongSanPham'] + '</option>');
            });

        }
    });
});
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
//Load Image Preview for shoe create 
function preview_image() {
    var ii = document.querySelectorAll("#imgPreview > img");
    if (ii.length == 0) {
        var total_file = document.getElementById("imageBrowes").files.length;
        for (var i = 0; i < total_file; i++) {
            $('#imgPreview').append("<img style='margin-top: 10px; width: 60px; height: 60px;' src='" + URL.createObjectURL(event.target.files[i]) + "'><br>");
        }
    } else {
        var hh = document.querySelectorAll("#imgPreview > img");
        hh.forEach(e => e.remove());
        var total_file = document.getElementById("imageBrowes").files.length;
        for (var i = 0; i < total_file; i++) {
            $('#imgPreview').append("<img style='margin-top: 10px; width: 60px; height: 60px;' src='" + URL.createObjectURL(event.target.files[i]) + "'><br>");
        }
    }
    var hidden = document.getElementById('image-display');
    hidden.style.display = "none";
}
//Preview images array
function preview_images() {
    var ii = document.querySelectorAll("#image_preview > img");
    if (ii.length == 0) {
        var total_file = document.getElementById("imagesBrowes").files.length;
        for (var i = 0; i < total_file; i++) {
            $('#image_preview').append("<img style='margin-top: 10px; width: 60px; height: 60px;' src='" + URL.createObjectURL(event.target.files[i]) + "'><br>");
        }
    } else {
        var hh = document.querySelectorAll("#image_preview > img");
        hh.forEach(e => e.remove());
        var total_file = document.getElementById("imagesBrowes").files.length;
        for (var i = 0; i < total_file; i++) {
            $('#image_preview').append("<img style='margin-top: 10px; width: 60px; height: 60px;' src='" + URL.createObjectURL(event.target.files[i]) + "'><br>");
        }
    }
    var hidden = document.getElementById('images-display');
    hidden.style.display = "none";
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
                else {
                    $('#form-modal .modal-body').html(res.html);
                }
                   
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
            x[cnt].value = '';
            if (x[cnt].id.indexOf('__MaSize') > 0)
                x[cnt].value = 'Vui long chon size giay';
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
    txtIsDeleted.value = "false";
    $(btn).closest('tr').hide();

    CalcTotals();

}
function setSameWidth(srcElement, desElement) {
    desElement.style.width = "360px";
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
            if (eval(x[i].value) === undefined)
                x[i].value = 0;
            totalQty = totalQty + eval(x[i].value);
            var txttotal = document.getElementById(totalTxtId);
            var txtprice = document.getElementById(priceTxtId);
            if (isNaN(totalQty)) {
                totalQty = eval(x[i].value);
            }
            var Total = eval(x[i].value) * txtprice.value;
            if (isNaN(Total))
                txttotal.value = '';
            else
                txttotal.value = Total;
            totalAmount = eval(totalAmount) + eval(txttotal.value);

        }
    }
    if (isNaN(totalAmount))
        totalAmount = '';
    document.getElementById('txtQtyTotal').value = totalQty;
    document.getElementById('txtAmountTotal').value = totalAmount;

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
    for (var i = 0; i < items.length; i++) {
        console.log(items[i].value);
    }
    for (var i = items.length - 1; i >= 0; i--) {
        if (items[i].value.toLowerCase().indexOf(txtSearch.value.toLowerCase()) > -1) {
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
//get collection adidas
$.ajax({
    url: '/adidas/GetCollection',
    success: function (result) {
        $('#Adidas').append('<li><a href="/adidas">' + "TẤT CẢ ADIDAS" + '</a></li>');
        $.each(result, function (i, data) {
            var item = data['tenDongSanPham'];
            $('#Adidas').append('<li><a href="/adidas/' + item +'">' + data['tenDongSanPham'] + '</a></li>');
        });
    },
    error: function (err) {
        console.log(err);
    }
});
//Get collection Converse
$.ajax({
    url: '/converse/GetCollection',
    success: function (result) {
        $('#Converse').append(' <li><a href="/converse">' + "TẤT CẢ CONVERSE" + '</a></li>');
        $.each(result, function (i, data) {
            var item = data['tenDongSanPham'];
            $('#Converse').append('<li><a href="/converse/' + item + '">' + data['tenDongSanPham'] + '</a></li>');
        });
    },
    error: function (err) {
        console.log(err);
    }
});
//Get collection Nike
$.ajax({
    url: '/nike/GetCollection',
    success: function (result) {
        $('#Nike').append(' <li><a href="/nike">' + "TẤT CẢ NIKE" + '</a></li>');
        $.each(result, function (i, data) {
            var item = data['tenDongSanPham'];
            $('#Nike').append('<li><a href="/nike/' + item + '">' + data['tenDongSanPham'] + '</a></li>');
        });
    },
    error: function (err) {
        console.log(err);
    }
});
//Get Collection Vans
$.ajax({
    url: '/vans/GetCollection',
    success: function (result) {
        $('#Vans').append(' <li><a href="/vans">' + "TẤT CẢ VANS" + '</a></li>');
        $.each(result, function (i, data) {
            var item = data['tenDongSanPham'];
            $('#Vans').append('<li><a href="/vans/' + item + '">' + data['tenDongSanPham'] + '</a></li>');
        });
    },
    error: function (err) {
        console.log(err);
    }
});