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
        // Formdaki deðerleri al
        var empName = $("#emp_name").val();
        var empSurname = $("#emp_surname").val();
        var empPhone = $("#emp_phone").val();
        var empMail = $("#emp_mail").val();
        var empStart = $("#emp_start").val();
        var empEnd = $("#emp_end").val();
        var empSalary = $("#emp_salary").val();

        // Veriyi bir nesne olarak hazýrla
        var employeeData = {
            Name: empName,
            Surname: empSurname,
            Phone: empPhone,
            Email: empMail,
            StartDate: empStart,
            EndDate: empEnd,
            Salary: empSalary
            // Diðer özellikleri de ekleyebilirsiniz
        };

        // AJAX isteði gönder
        $.ajax({
            type: "POST",
            url: "/Employee/AddEmployee", // Bu URL'yi kendi projenize göre ayarlayýn
            data: JSON.stringify(employeeData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    // Baþarý durumunda yapýlacak iþlemler
                    alert(response.message);
                } else {
                    // Hata durumunda yapýlacak iþlemler
                    alert("Hata: " + response.message);
                }
            },
            error: function (error) {
                console.error("AJAX hatasý", error);
            }
        });
    });
});