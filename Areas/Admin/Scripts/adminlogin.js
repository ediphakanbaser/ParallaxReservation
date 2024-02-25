$("#login-btn").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();

    // AJAX kullanarak HTTP POST iste�i g�nderme
    $.post("/Admin/Login", { Username: username, Password: password }, function (data) {
        if (data.success) {
            // Ba�ar�yla tamamland���nda yap�lacak i�lemler
            console.log(data);
            // Sayfay� y�nlendir
            window.location.href = data.redirectUrl;
        } else {
            // Hata durumunda yap�lacak i�lemler
            console.log("Giri� ba�ar�s�z!");
        }
    });
});