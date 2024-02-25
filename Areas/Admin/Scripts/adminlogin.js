$("#login-btn").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();

    // AJAX kullanarak HTTP POST isteði gönderme
    $.post("/Admin/Login", { Username: username, Password: password }, function (data) {
        if (data.success) {
            // Baþarýyla tamamlandýðýnda yapýlacak iþlemler
            console.log(data);
            // Sayfayý yönlendir
            window.location.href = data.redirectUrl;
        } else {
            // Hata durumunda yapýlacak iþlemler
            console.log("Giriþ baþarýsýz!");
        }
    });
});