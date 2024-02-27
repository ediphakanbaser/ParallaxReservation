$(document).ready(function () {
    $("#btn-about").on("click", function () {
        var aboutText = $("#abouttext").val();
        console.log(aboutText);
        $.ajax({
            type: "POST",
            url: "/Dashboard/UpdateAboutText",
            data: { AboutText: aboutText }, // Modelin property ismi ile e�le�meli
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    console.log(response.message);
                    $("#abouttext").val(response.updatedAboutText);
                    // Ba�ar� durumunda yap�lacak i�lemler
                }
                else {
                    console.error(response.message);
                    // Hata durumunda yap�lacak i�lemler
                }
            },
            error: function (error) {
                console.error("AboutText g�ncelleme hatas�", error);
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
                        // Ba�ar� durumunda yap�lacak i�lemler
                    }
                    else {
                        console.error(response.message);
                        // Hata durumunda yap�lacak i�lemler
                    }
                },
                error: function (error) {
                    console.error("G�rsel g�ncelleme hatas�", error);
                }
            });
        } else {
            console.error("L�tfen ge�erli bir g�rsel se�in");
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
            console.error("Ge�ersiz alan");
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
                // Ba�ar� durumunda yap�lacak i�lemler
            } else {
                console.error(response.message);
                // Hata durumunda yap�lacak i�lemler
            }
        },
        error: function (error) {
            console.error("Discount g�ncelleme hatas�", error);
        }
    });
}

function updateShift(field) {
    var value;

    // �lgili input eleman�ndan de�eri al
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
            console.error("Ge�ersiz alan");
            return;
    }
    console.log(value);
    // Ajax iste�i
    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateShift",
        data: { field: field, timeSpanValue: value },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                console.log(response.message);
                // Ba�ar� durumunda yap�lacak i�lemler
            } else {
                console.error(response.message);
                // Hata durumunda yap�lacak i�lemler
            }
        },
        error: function (error) {
            console.error("Shift g�ncelleme hatas�", error);
        }
    });
}


$(document).ready(function () {
    $("#btn-emp").on("click", function () {
        // Formdaki de�erleri al
        var empName = $("#emp_name").val();
        var empSurname = $("#emp_surname").val();
        var empPhone = $("#emp_phone").val();
        var empMail = $("#emp_mail").val();
        var empStart = $("#emp_start").val();
        var empEnd = $("#emp_end").val();
        var empSalary = $("#emp_salary").val();

        // Veriyi bir nesne olarak haz�rla
        var employeeData = {
            Name: empName,
            Surname: empSurname,
            Phone: empPhone,
            Email: empMail,
            StartDate: empStart,
            EndDate: empEnd,
            Salary: empSalary
            // Di�er �zellikleri de ekleyebilirsiniz
        };

        // AJAX iste�i g�nder
        $.ajax({
            type: "POST",
            url: "/Employee/AddEmployee", // Bu URL'yi kendi projenize g�re ayarlay�n
            data: JSON.stringify(employeeData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    // Ba�ar� durumunda yap�lacak i�lemler
                    alert(response.message);
                } else {
                    // Hata durumunda yap�lacak i�lemler
                    alert("Hata: " + response.message);
                }
            },
            error: function (error) {
                console.error("AJAX hatas�", error);
            }
        });
    });
});