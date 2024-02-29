document.getElementById('btnCalendarModal').addEventListener('click', function () {
    document.getElementById('fullScreenModal').style.display = 'flex';

});

document.getElementById('closeModalBtn').addEventListener('click', function () {
    document.getElementById('fullScreenModal').style.display = 'none';

});

// Arka plana týklanýnca modalýn kapanmasý
$(document).ready(function () {
    // Modal açýldýðýnda
    $("#btnCalendarModal").on("click", function () {
        
        $("body").css("overflow", "hidden"); // Body'ye overflow: hidden; ekleyerek scroll'u engelle
    });

    // Modal kapatýldýðýnda
    $("#closeModalBtn").on("click", function () {
        
        $("body").css("overflow", "auto"); // Body'ye overflow: auto; ekleyerek scroll'u serbest býrak
    });
});

$(document).ready(function () {
    $("#rsvCancel").on("click", function () {
        // Seçilmiþ radio butonunu bul
        var selectedRadio = $("input[name='options']:checked");

        // Seçili bir radio butonu var mý kontrol et
        if (selectedRadio.length > 0) {
            // Seçili radio butonunun deðerini al
            var reservationID = selectedRadio.val();

            // AJAX isteði gönder
            $.ajax({
                type: "POST",
                url: "/Reservation/DeleteReservation", // Ýstek yapýlacak controller ve action'ý belirtin
                data: { reservationID: reservationID },
                success: function (result) {
                    // Ýstek baþarýlý olduðunda yapýlacak iþlemler
                    console.log("Rezervasyon iptal edildi.");
                },
                error: function () {
                    // Ýstek hatasý olduðunda yapýlacak iþlemler
                    console.error("Rezervasyon iptal edilemedi. Bir hata oluþtu.");
                }
            });
        } else {
            // Seçili bir radio butonu yoksa kullanýcýya uyarý ver
            alert("Lütfen bir rezervasyon seçin.");
        }
    });
});