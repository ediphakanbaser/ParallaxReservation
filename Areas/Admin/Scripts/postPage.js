$(document).ready(function () {
    $("#btn-about").on("click", function () {
        var aboutText = $("#abouttext").val();
        console.log(aboutText);
        $.ajax({
            type: "POST",
            url: "/Dashboard/UpdateAboutText",
            data: { AboutText: aboutText }, // Modelin property ismi ile eþleþmeli
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    console.log(response.message);
                    $("#abouttext").val(response.updatedAboutText);
                    // Baþarý durumunda yapýlacak iþlemler
                }
                else {
                    console.error(response.message);
                    // Hata durumunda yapýlacak iþlemler
                }
            },
            error: function (error) {
                console.error("AboutText güncelleme hatasý", error);
            }
        });
    });
});

$(document).ready(function () {
    $(".btn-upload").on("click", function () {
        var inputId = $(this).data("input");
        var inputFile = $(inputId)[0].files[0];
        var url = $(this).data("url");

        if (inputFile) {
            var formData = new FormData();
            formData.append("image", inputFile);

            $.ajax({
                type: "POST",
                url: url,
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.success) {
                        console.log(response.message);
                        // Baþarý durumunda yapýlacak iþlemler
                    }
                    else {
                        console.error(response.message);
                        // Hata durumunda yapýlacak iþlemler
                    }
                },
                error: function (error) {
                    console.error("Görsel güncelleme hatasý", error);
                }
            });
        } else {
            console.error("Lütfen geçerli bir görsel seçin");
        }
    });
});


function updateDiscount(field) {
    var value;

    switch (field) {
        case 'title':
            value = $("#discountTitle").val();
            break;
        case 'paragraph':
            value = $("#discountParagraph").val();
            break;
        case 'amount':
            value = $("#discountAmount").val();
            break;
        default:
            console.error("Geçersiz alan");
            return;
    }

    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateDiscount",
        data: { field: field, value: value },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                console.log(response.message);
                // Baþarý durumunda yapýlacak iþlemler
            } else {
                console.error(response.message);
                // Hata durumunda yapýlacak iþlemler
            }
        },
        error: function (error) {
            console.error("Discount güncelleme hatasý", error);
        }
    });
}

function updateShift(field) {
    var value;

    // Ýlgili input elemanýndan deðeri al
    switch (field) {
        case 'mesaiBaslangic':
            value = $("#mesaiBaslangic").val() + ":00";
            break;
        case 'molaBaslangic':
            value = $("#molaBaslangic").val() + ":00";
            break;
        case 'molaBitis':
            value = $("#molaBitis").val() + ":00";
            break;
        case 'mesaiBitis':
            value = $("#mesaiBitis").val() + ":00";
            break;
        default:
            console.error("Geçersiz alan");
            return;
    }
    console.log(value);
    // Ajax isteði
    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateShift",
        data: { field: field, timeSpanValue: value },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                console.log(response.message);
                // Baþarý durumunda yapýlacak iþlemler
            } else {
                console.error(response.message);
                // Hata durumunda yapýlacak iþlemler
            }
        },
        error: function (error) {
            console.error("Shift güncelleme hatasý", error);
        }
    });
}


$(document).ready(function () {
    $("#btn-emp").on("click", function () {
        var formData = new FormData();
        var fileInput = $("#emp_img")[0].files[0];

        formData.append("EmpImage", fileInput);
        formData.append("EmpName", $("#emp_name").val());
        formData.append("EmpSurname", $("#emp_surname").val());
        formData.append("EmpPhone", $("#emp_phone").val());
        formData.append("EmpMail", $("#emp_mail").val());
        formData.append("EmpSalary", parseFloat($("#emp_salary").val()));
        formData.append("EmpStart", $("#emp_start").val());
        formData.append("EmpEnd", $("#emp_end").val());
        formData.append("SKILLS", null);

        $.ajax({
            type: "POST",
            url: "/Dashboard/AddEmployee", // Controller ve Action isminizi buraya ekleyin
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                console.log(response);
                // Baþarýlý bir þekilde çalýþan eklenirse yapýlacak iþlemler
            },
            error: function (error) {
                console.error(error);
                // Hata durumunda yapýlacak iþlemler
            }
        });
    });
});

$(document).ready(function () {
    // Özel bir class ("btnEditEmployee") kullanarak buton týklama olayýný dinle
    $(".btnEditEmployee").on("click", function () {
        event.preventDefault();
        var employeeID = $(this).val(); // Týklanan butonun value deðeri (EmployeeID)

        // AJAX isteði ile çalýþan bilgilerini getir
        $.ajax({
            type: "GET",
            url: "/Dashboard/GetEmployeeInfo",
            data: { employeeID: employeeID },
            success: function (employeeInfo) {
                // Bilgileri form alanlarýna doldur                
                $("#emp_title").text(employeeInfo.EmpID);
                $("#upd_name").val(employeeInfo.EmpName);
                $("#upd_surname").val(employeeInfo.EmpSurname);
                $("#upd_phone").val(employeeInfo.EmpPhone);
                $("#upd_mail").val(employeeInfo.EmpMail);
                // Çalýþma Baþlama Tarihi kontrolü
                var empStart = employeeInfo.EmpStardDate ? formatDate(employeeInfo.EmpStardDate) : '';
                $("#upd_start").val(empStart);

                // Çalýþma Bitiþ Tarihi kontrolü
                var empEnd = employeeInfo.EmpDismissalDate ? formatDate(employeeInfo.EmpDismissalDate) : '';
                $("#upd_end").val(empEnd);
                function formatDate(input) {
                    var date = new Date(parseInt(input.substr(6)));
                    var formattedDate = date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);
                    return formattedDate;
                }
                $("#upd_salary").val(employeeInfo.EmpSalary);

                // Ýsteðe baðlý olarak resim önizlemesi de yapýlabilir
                if (employeeInfo.EmpImageURL) {
                    $("#previewUpdate").html("<img src='" + employeeInfo.EmpImageURL + "' alt='Employee Image' width='40' height='30' />");
                } else {
                    // Eðer resim URL'si yoksa önizlemeyi temizle
                    $("#previewUpdate").empty();
                }
            },
            error: function (error) {
                console.error(error);
            }
        });
    });
});

$("#btn-upd").on("click", function () {
    // Burada güncelleme iþlemlerini gerçekleþtirebilirsiniz
    var formData = new FormData();
    var fileInput = $("#update_img")[0].files[0];

    if (fileInput) {
        // Yeni bir resim dosyasý seçildiyse
        formData.append("EmpImage", fileInput);
    }

    formData.append("EmpID", parseInt($("#emp_title").text()));
    formData.append("EmpName", $("#upd_name").val());
    formData.append("EmpSurname", $("#upd_surname").val());
    formData.append("EmpPhone", $("#upd_phone").val());
    formData.append("EmpMail", $("#upd_mail").val());
    formData.append("EmpSalary", parseFloat($("#upd_salary").val()));
    formData.append("EmpStart", $("#upd_start").val());
    formData.append("EmpEnd", $("#upd_end").val());
    formData.append("SKILLS", null);

    // Güncellenmiþ verileri al
    var updatedData = {
        EmpID: $("#emp_title").text(),
        EmpName: $("#upd_name").val(),
        EmpSurname: $("#upd_surname").val(),
        EmpPhone: $("#upd_phone").val(),
        EmpMail: $("#upd_mail").val(),
        EmpStart: $("#upd_start").val(),
        EmpEnd: $("#upd_end").val(),
        // Diðer alanlarý ekleyin
    };

    // AJAX isteði ile güncelleme iþlemini gerçekleþtir
    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateEmployee",
        contentType: false, // Dosya yükleme iþlemi olduðu için false
        processData: false, // Dosya yükleme iþlemi olduðu için false
        data: formData,
        success: function (response) {
            console.log(response);
            // Baþarýlý bir þekilde güncellendiðini iþaretlemek için gerekli iþlemleri yapabilirsiniz
        },
        error: function (error) {
            console.error(error);
            // Güncelleme sýrasýnda hata oluþtuðunda gerekli iþlemleri yapabilirsiniz
        }
    });
});