$(document).ready(function () {
    $("#btn-about").on("click", function () {
        var aboutText = $("#abouttext").val();

        $.ajax({
            type: "POST",
            url: "/Dashboard/UpdateAboutText",
            data: { AboutText: aboutText }, // Modelin property ismi ile e�le�meli
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $("#abouttext").val(response.updatedAboutText);
                    // Ba�ar� durumunda yap�lacak i�lemler
                }
                else {
                    alert(response.message);
                    // Hata durumunda yap�lacak i�lemler
                }
            },
            error: function (error) {
                alert("AboutText g�ncelleme hatas�: " + error);
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
                        alert(response.message);
                        // Ba�ar� durumunda yap�lacak i�lemler
                    }
                    else {
                        alert(response.message);
                        // Hata durumunda yap�lacak i�lemler
                    }
                },
                error: function (error) {
                    alert("G�rsel g�ncelleme hatas�: " + error);
                }
            });
        } else {
            alert("L�tfen ge�erli bir g�rsel se�in");
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
            alert("Ge�ersiz alan");
            return;
    }

    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateDiscount",
        data: { field: field, value: value },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                alert(response.message);
                // Ba�ar� durumunda yap�lacak i�lemler
            } else {
                alert(response.message);
                // Hata durumunda yap�lacak i�lemler
            }
        },
        error: function (error) {
            alert("Discount g�ncelleme hatas�: " + error);
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
            alert("Ge�ersiz alan");
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
                alert(response.message);
                // Ba�ar� durumunda yap�lacak i�lemler
            } else {
                alert(response.message);
                // Hata durumunda yap�lacak i�lemler
            }
        },
        error: function (error) {
            alert("Shift g�ncelleme hatas�: " + error);
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
                alert(response);
                // Ba�ar�l� bir �ekilde �al��an eklenirse yap�lacak i�lemler
            },
            error: function (error) {
                alert(response);
                // Hata durumunda yap�lacak i�lemler
            }
        });
    });
});

$(document).ready(function () {
    // �zel bir class ("btnEditEmployee") kullanarak buton t�klama olay�n� dinle
    $(".btnEditEmployee").on("click", function () {
        event.preventDefault();
        var employeeID = $(this).val(); // T�klanan butonun value de�eri (EmployeeID)

        // AJAX iste�i ile �al��an bilgilerini getir
        $.ajax({
            type: "GET",
            url: "/Dashboard/GetEmployeeInfo",
            data: { employeeID: employeeID },
            success: function (employeeInfo) {
                // Bilgileri form alanlar�na doldur                
                $("#emp_title").text(employeeInfo.EmpID);
                $("#upd_name").val(employeeInfo.EmpName);
                $("#upd_surname").val(employeeInfo.EmpSurname);
                $("#upd_phone").val(employeeInfo.EmpPhone);
                $("#upd_mail").val(employeeInfo.EmpMail);
                // �al��ma Ba�lama Tarihi kontrol�
                var empStart = employeeInfo.EmpStardDate ? formatDate(employeeInfo.EmpStardDate) : '';
                $("#upd_start").val(empStart);

                // �al��ma Biti� Tarihi kontrol�
                var empEnd = employeeInfo.EmpDismissalDate ? formatDate(employeeInfo.EmpDismissalDate) : '';
                $("#upd_end").val(empEnd);
                function formatDate(input) {
                    var date = new Date(parseInt(input.substr(6)));
                    var formattedDate = date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);
                    return formattedDate;
                }
                $("#upd_salary").val(employeeInfo.EmpSalary);

                // �ste�e ba�l� olarak resim �nizlemesi de yap�labilir
                if (employeeInfo.EmpImageURL) {
                    $("#previewUpdate").html("<img src='" + employeeInfo.EmpImageURL + "' alt='Employee Image' width='40' height='30' />");
                } else {
                    // E�er resim URL'si yoksa �nizlemeyi temizle
                    $("#previewUpdate").empty();
                }
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});

$("#btn-upd").on("click", function () {
    // Burada g�ncelleme i�lemlerini ger�ekle�tirebilirsiniz
    var formData = new FormData();
    var fileInput = $("#update_img")[0].files[0];

    if (fileInput) {
        // Yeni bir resim dosyas� se�ildiyse
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

    // G�ncellenmi� verileri al

    // AJAX iste�i ile g�ncelleme i�lemini ger�ekle�tir
    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateEmployee",
        contentType: false, // Dosya y�kleme i�lemi oldu�u i�in false
        processData: false, // Dosya y�kleme i�lemi oldu�u i�in false
        data: formData,
        success: function (response) {
            alert(response);
            location.reload();
            // Ba�ar�l� bir �ekilde g�ncellendi�ini i�aretlemek i�in gerekli i�lemleri yapabilirsiniz
        },
        error: function (error) {
            alert(error);
            // G�ncelleme s�ras�nda hata olu�tu�unda gerekli i�lemleri yapabilirsiniz
        }
    });
});



$(document).ready(function () {
    $("#btn-srv").on("click", function (event) {
        event.preventDefault();

        // Dosya se�imi kontrol� yap�n
        var imgInput = $("#serv_img")[0].files[0];
        if (imgInput) {
            var formData = new FormData();
            var srvName = $("#serv_name").val();
            var srvTypeValue = $("#serv_type").val();
            var srvSpent = $("#serv_spt").val() + ":00";
            var srvPrice = parseFloat($("#serv_price").val());
            var srvDisc = parseFloat($("#serv_disc").val());
            var srvPrg = $("#serv_prg").val();

            

            formData.append("SrvImage", imgInput);
            formData.append("SrvName", srvName);
            formData.append("SrvType", srvTypeValue);
            formData.append("SrvSpent", srvSpent);
            formData.append("SrvPrice", srvPrice);
            formData.append("SrvDisc", srvDisc);
            formData.append("SrvPrg", srvPrg);

            console.log(formData);

            $.ajax({
                type: "POST",
                url: "/Dashboard/AddService",
                data: formData,
                contentType: false,
                processData: false,

                success: function (response) {
                    alert(response);
                    location.reload();
                    // Ba�ar�l� bir �ekilde �al��t���nda yap�lacak i�lemler
                },
                error: function (error) {
                    alert(error);
                    // Hata durumunda yap�lacak i�lemler
                }
            });
        } else {
            alert("Dosya se�ilmedi veya ��e bulunamad�.");
        }
    });
});

$(document).ready(function () {
// �zel bir class ("btnEditEmployee") kullanarak buton t�klama olay�n� dinle
    $(".btnEditService").on("click", function () {
        event.preventDefault();
        var serviceID = $(this).val(); // T�klanan butonun value de�eri (EmployeeID)

        $.ajax({
            type: "GET",
            url: "/Dashboard/GetServiceInfo",
            data: { serviceID: serviceID },
            success: function (serviceInfo) {
                // Bilgileri form alanlar�na doldur                
                $("#servup_title").text(serviceInfo.SrvID);
                $("#servup_name").val(serviceInfo.SrvName);
                $("#servup_type").val(serviceInfo.SrvType);
                
                $("#servup_price").val(serviceInfo.SrvPrice);
                $("#servup_disc").val(serviceInfo.SrvDisc);
                $("#servup_prg").val(serviceInfo.SrvPrg);
                // �al��ma Ba�lama Tarihi kontrol�
                var hours = serviceInfo.SrvSpent.Hours;
                var minutes = serviceInfo.SrvSpent.Minutes;

                // Saat ve dakika bilgilerini 10'dan k���kse ba�lar�na 0 ekleyerek birle�tir
                var formattedHours = hours < 10 ? '0' + hours : hours;
                var formattedMinutes = minutes < 10 ? '0' + minutes : minutes;

                // Saat ve dakika bilgilerini birle�tirerek formatlayabilirsiniz
                var formattedTime = formattedHours + ':' + formattedMinutes;

                // input alan�n�n value �zelli�ini g�ncelle
                $("#servup_spt").val(formattedTime);
               
                console.log(formattedTime);


                if (serviceInfo.SrvImageURL) {
                    $("#previewServiceUp").html("<img src='" + serviceInfo.SrvImageURL + "' alt='Employee Image' width='40' height='30' />");
                } else {
                    // E�er resim URL'si yoksa �nizlemeyi temizle
                    $("#previewServiceUp").empty();
                }
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});

$(document).ready(function () {
    $(".btnChangeService").on("click", function () {
        var serviceID = $(this).val();

        // AJAX iste�i ile sunucuya durumu g�ncelle
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChangeServiceStatus", // Bu URL'yi kendi projenize uygun �ekilde g�ncelleyin
            data: { serviceID: serviceID },
            success: function (response) {
                if (response.success) {
                    // Sunucu taraf�nda ba�ar�l� bir �ekilde g�ncellendi
                    alert(response.message);
                    location.reload();
                } else {
                    // Sunucu taraf�nda bir hata olu�tu
                    alert(response.message);
                }
            },
            error: function (error) {
                alert(error);
            }
        });
    });
});

$("#btn-servup").on("click", function () {
    // Burada g�ncelleme i�lemlerini ger�ekle�tirebilirsiniz
    var formData = new FormData();
    var fileInput = $("#servup_img")[0].files[0];

    if (fileInput) {
        // Yeni bir resim dosyas� se�ildiyse
        formData.append("SrvImage", fileInput);
    }
    formData.append("SrvID", parseInt($("#servup_title").text()));
    formData.append("SrvName", $("#servup_name").val());
    formData.append("SrvType", $("#servup_type").val());
    formData.append("SrvSpent", $("#servup_spt").val() + ":00");
    formData.append("SrvPrice", parseFloat($("#servup_price").val()));
    formData.append("SrvDisc", parseFloat($("#servup_disc").val()));
    formData.append("SrvPrg", $("#servup_prg").val());
 

    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateService",
        contentType: false, // Dosya y�kleme i�lemi oldu�u i�in false
        processData: false, // Dosya y�kleme i�lemi oldu�u i�in false
        data: formData,
        success: function (response) {
            alert(response);
            location.reload();
            // Ba�ar�l� bir �ekilde g�ncellendi�ini i�aretlemek i�in gerekli i�lemleri yapabilirsiniz
        },
        error: function (error) {
            alert(error);
            // G�ncelleme s�ras�nda hata olu�tu�unda gerekli i�lemleri yapabilirsiniz
        }
    });
});

$(document).ready(function () {
    $(".employeeSelector").on("click", function () {
        var selectedEmployeeId = $(this).val();
        $(".hiddenId").text(selectedEmployeeId);
    });

    $(".serviceAdd").on("click", function () {
        var selectedEmployeeId = $(".hiddenId").text();
        var selectedServiceId = $(this).val();

        $.ajax({
            url: "/Dashboard/ServiceAdd", // ServisEkle metodu bulundu�u controller ad� ile de�i�tirilmelidir
            type: "POST",
            data: {
                employeeId: selectedEmployeeId,
                serviceId: selectedServiceId
            },
            success: function (data) {
                alert(data.message);
            },
            error: function (error) {
                alert("Hata olu�tu: " + error);
            }
        });
    });

    $(".serviceDelete").on("click", function () {
        var selectedEmployeeId = $(".hiddenId").text();
        var selectedServiceId = $(this).val();

        $.ajax({
            url: "/Dashboard/ServiceDelete", // ServisKald�r metodu bulundu�u controller ad� ile de�i�tirilmelidir
            type: "POST",
            data: {
                employeeId: selectedEmployeeId,
                serviceId: selectedServiceId
            },
            success: function (data) {
                alert(data.message);
            },
            error: function (error) {
                console.error(error.message);
            }
        });
    });
});

$(document).ready(function () {
    $(".decisionReview").click(function () {
        // Se�ilen sat�r�n de�erlerini al        
        var reservationID = $(this).val(); // ReservationID'yi al

        // AJAX iste�i
        $.ajax({
            url: "/Dashboard/GetReviewInfo", // Controller action'�n ad�
            type: "POST",
            data: {
                reservationID: reservationID // reservationID'yi g�nder
            },
            success: function (data) {
                // AJAX ba�ar�l� oldu�unda i�lemleri ger�ekle�tir
                // Bu k�s�mda ald���n�z verileri ekrana yerle�tirebilirsiniz
                $("#reservationId").val(reservationID);
                $("#reviewPrg").val(data.comment);
                $("#reviewId").val(data.reviewID);
                $(".rating-review").val(data.rating);
            },
            error: function (error) {
                // Hata durumunda i�lemleri ger�ekle�tir
                console.error("Hata olu�tu: " + error.responseText);
            }
        });
    });
});

$(document).ready(function () {
    $("#commitReview").click(function () {
        

        // De�erleri do�rudan jQuery val() fonksiyonu ile �ekin
        var reviewID = $("#reviewId").val();
        var reservationID = $("#reservationId").val();
        var rating = $(".rating-review").val();
        var comment = $("#reviewPrg").val();

        console.log(reviewID)
        console.log(reservationID)
        console.log(rating)
        console.log(comment)

        // AJAX iste�i
        $.ajax({
            url: "/Dashboard/CommitReview",
            type: "POST",
            data: {
                reviewID: reviewID,
                reservationID: reservationID,
                rating: rating,
                comment: comment
            },
            success: function (data) {
                // Ba�ar�l� oldu�unda i�lemleri ger�ekle�tir
                alert(data.message);
                location.reload();
            },
            error: function (error) {
                // Hata durumunda i�lemleri ger�ekle�tir
                alert("Hata olu�tu: " + error.responseText);
            }
        });
    });
});

$(document).ready(function () {
    $("#dropReview").click(function () {
        var reservationID = $("#reservationId").val();
        console.log(reservationID)


        // AJAX iste�i
        $.ajax({
            url: "/Dashboard/DropReview",
            type: "POST",
            data: {                
                reservationID: reservationID,
            },
            success: function (data) {
                // Ba�ar�l� oldu�unda i�lemleri ger�ekle�tir
                alert(data.message);
                location.reload();
            },
            error: function (error) {
                // Hata durumunda i�lemleri ger�ekle�tir
                alert("Hata olu�tu: " + error.responseText);
            }
        });
    });
});