$(document).ready(function () {
    $("#btn-about").on("click", function () {
        var aboutText = $("#abouttext").val();

        $.ajax({
            type: "POST",
            url: "/Dashboard/UpdateAboutText",
            data: { AboutText: aboutText }, // Modelin property ismi ile eþleþmeli
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $("#abouttext").val(response.updatedAboutText);
                    // Baþarý durumunda yapýlacak iþlemler
                }
                else {
                    alert(response.message);
                    // Hata durumunda yapýlacak iþlemler
                }
            },
            error: function (error) {
                alert("AboutText güncelleme hatasý: " + error);
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
                        // Baþarý durumunda yapýlacak iþlemler
                    }
                    else {
                        alert(response.message);
                        // Hata durumunda yapýlacak iþlemler
                    }
                },
                error: function (error) {
                    alert("Görsel güncelleme hatasý: " + error);
                }
            });
        } else {
            alert("Lütfen geçerli bir görsel seçin");
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
            alert("Geçersiz alan");
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
                // Baþarý durumunda yapýlacak iþlemler
            } else {
                alert(response.message);
                // Hata durumunda yapýlacak iþlemler
            }
        },
        error: function (error) {
            alert("Discount güncelleme hatasý: " + error);
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
            alert("Geçersiz alan");
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
                alert(response.message);
                // Baþarý durumunda yapýlacak iþlemler
            } else {
                alert(response.message);
                // Hata durumunda yapýlacak iþlemler
            }
        },
        error: function (error) {
            alert("Shift güncelleme hatasý: " + error);
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
                // Baþarýlý bir þekilde çalýþan eklenirse yapýlacak iþlemler
            },
            error: function (error) {
                alert(response);
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
                alert(error);
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

    // AJAX isteði ile güncelleme iþlemini gerçekleþtir
    $.ajax({
        type: "POST",
        url: "/Dashboard/UpdateEmployee",
        contentType: false, // Dosya yükleme iþlemi olduðu için false
        processData: false, // Dosya yükleme iþlemi olduðu için false
        data: formData,
        success: function (response) {
            alert(response);
            location.reload();
            // Baþarýlý bir þekilde güncellendiðini iþaretlemek için gerekli iþlemleri yapabilirsiniz
        },
        error: function (error) {
            alert(error);
            // Güncelleme sýrasýnda hata oluþtuðunda gerekli iþlemleri yapabilirsiniz
        }
    });
});



$(document).ready(function () {
    $("#btn-srv").on("click", function (event) {
        event.preventDefault();

        // Dosya seçimi kontrolü yapýn
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
                    // Baþarýlý bir þekilde çalýþtýðýnda yapýlacak iþlemler
                },
                error: function (error) {
                    alert(error);
                    // Hata durumunda yapýlacak iþlemler
                }
            });
        } else {
            alert("Dosya seçilmedi veya öðe bulunamadý.");
        }
    });
});

$(document).ready(function () {
// Özel bir class ("btnEditEmployee") kullanarak buton týklama olayýný dinle
    $(".btnEditService").on("click", function () {
        event.preventDefault();
        var serviceID = $(this).val(); // Týklanan butonun value deðeri (EmployeeID)

        $.ajax({
            type: "GET",
            url: "/Dashboard/GetServiceInfo",
            data: { serviceID: serviceID },
            success: function (serviceInfo) {
                // Bilgileri form alanlarýna doldur                
                $("#servup_title").text(serviceInfo.SrvID);
                $("#servup_name").val(serviceInfo.SrvName);
                $("#servup_type").val(serviceInfo.SrvType);
                
                $("#servup_price").val(serviceInfo.SrvPrice);
                $("#servup_disc").val(serviceInfo.SrvDisc);
                $("#servup_prg").val(serviceInfo.SrvPrg);
                // Çalýþma Baþlama Tarihi kontrolü
                var hours = serviceInfo.SrvSpent.Hours;
                var minutes = serviceInfo.SrvSpent.Minutes;

                // Saat ve dakika bilgilerini 10'dan küçükse baþlarýna 0 ekleyerek birleþtir
                var formattedHours = hours < 10 ? '0' + hours : hours;
                var formattedMinutes = minutes < 10 ? '0' + minutes : minutes;

                // Saat ve dakika bilgilerini birleþtirerek formatlayabilirsiniz
                var formattedTime = formattedHours + ':' + formattedMinutes;

                // input alanýnýn value özelliðini güncelle
                $("#servup_spt").val(formattedTime);
               
                console.log(formattedTime);


                if (serviceInfo.SrvImageURL) {
                    $("#previewServiceUp").html("<img src='" + serviceInfo.SrvImageURL + "' alt='Employee Image' width='40' height='30' />");
                } else {
                    // Eðer resim URL'si yoksa önizlemeyi temizle
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

        // AJAX isteði ile sunucuya durumu güncelle
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChangeServiceStatus", // Bu URL'yi kendi projenize uygun þekilde güncelleyin
            data: { serviceID: serviceID },
            success: function (response) {
                if (response.success) {
                    // Sunucu tarafýnda baþarýlý bir þekilde güncellendi
                    alert(response.message);
                    location.reload();
                } else {
                    // Sunucu tarafýnda bir hata oluþtu
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
    // Burada güncelleme iþlemlerini gerçekleþtirebilirsiniz
    var formData = new FormData();
    var fileInput = $("#servup_img")[0].files[0];

    if (fileInput) {
        // Yeni bir resim dosyasý seçildiyse
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
        contentType: false, // Dosya yükleme iþlemi olduðu için false
        processData: false, // Dosya yükleme iþlemi olduðu için false
        data: formData,
        success: function (response) {
            alert(response);
            location.reload();
            // Baþarýlý bir þekilde güncellendiðini iþaretlemek için gerekli iþlemleri yapabilirsiniz
        },
        error: function (error) {
            alert(error);
            // Güncelleme sýrasýnda hata oluþtuðunda gerekli iþlemleri yapabilirsiniz
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
            url: "/Dashboard/ServiceAdd", // ServisEkle metodu bulunduðu controller adý ile deðiþtirilmelidir
            type: "POST",
            data: {
                employeeId: selectedEmployeeId,
                serviceId: selectedServiceId
            },
            success: function (data) {
                alert(data.message);
            },
            error: function (error) {
                alert("Hata oluþtu: " + error);
            }
        });
    });

    $(".serviceDelete").on("click", function () {
        var selectedEmployeeId = $(".hiddenId").text();
        var selectedServiceId = $(this).val();

        $.ajax({
            url: "/Dashboard/ServiceDelete", // ServisKaldýr metodu bulunduðu controller adý ile deðiþtirilmelidir
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
        // Seçilen satýrýn deðerlerini al        
        var reservationID = $(this).val(); // ReservationID'yi al

        // AJAX isteði
        $.ajax({
            url: "/Dashboard/GetReviewInfo", // Controller action'ýn adý
            type: "POST",
            data: {
                reservationID: reservationID // reservationID'yi gönder
            },
            success: function (data) {
                // AJAX baþarýlý olduðunda iþlemleri gerçekleþtir
                // Bu kýsýmda aldýðýnýz verileri ekrana yerleþtirebilirsiniz
                $("#reservationId").val(reservationID);
                $("#reviewPrg").val(data.comment);
                $("#reviewId").val(data.reviewID);
                $(".rating-review").val(data.rating);
            },
            error: function (error) {
                // Hata durumunda iþlemleri gerçekleþtir
                console.error("Hata oluþtu: " + error.responseText);
            }
        });
    });
});

$(document).ready(function () {
    $("#commitReview").click(function () {
        

        // Deðerleri doðrudan jQuery val() fonksiyonu ile çekin
        var reviewID = $("#reviewId").val();
        var reservationID = $("#reservationId").val();
        var rating = $(".rating-review").val();
        var comment = $("#reviewPrg").val();

        console.log(reviewID)
        console.log(reservationID)
        console.log(rating)
        console.log(comment)

        // AJAX isteði
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
                // Baþarýlý olduðunda iþlemleri gerçekleþtir
                alert(data.message);
                location.reload();
            },
            error: function (error) {
                // Hata durumunda iþlemleri gerçekleþtir
                alert("Hata oluþtu: " + error.responseText);
            }
        });
    });
});

$(document).ready(function () {
    $("#dropReview").click(function () {
        var reservationID = $("#reservationId").val();
        console.log(reservationID)


        // AJAX isteði
        $.ajax({
            url: "/Dashboard/DropReview",
            type: "POST",
            data: {                
                reservationID: reservationID,
            },
            success: function (data) {
                // Baþarýlý olduðunda iþlemleri gerçekleþtir
                alert(data.message);
                location.reload();
            },
            error: function (error) {
                // Hata durumunda iþlemleri gerçekleþtir
                alert("Hata oluþtu: " + error.responseText);
            }
        });
    });
});