document.getElementById('btnCalendarModal').addEventListener('click', function () {
    document.getElementById('fullScreenModal').style.display = 'flex';

});

document.getElementById('closeModalBtn').addEventListener('click', function () {
    document.getElementById('fullScreenModal').style.display = 'none';

});

// Arka plana t�klan�nca modal�n kapanmas�
$(document).ready(function () {
    // Modal a��ld���nda
    $("#btnCalendarModal").on("click", function () {
        
        $("body").css("overflow", "hidden"); // Body'ye overflow: hidden; ekleyerek scroll'u engelle
    });

    // Modal kapat�ld���nda
    $("#closeModalBtn").on("click", function () {
        
        $("body").css("overflow", "auto"); // Body'ye overflow: auto; ekleyerek scroll'u serbest b�rak
    });
});

$(document).ready(function () {
    $("#rsvCancel").on("click", function () {
        // Se�ilmi� radio butonunu bul
        var selectedRadio = $("input[name='options']:checked");

        // Se�ili bir radio butonu var m� kontrol et
        if (selectedRadio.length > 0) {
            // Se�ili radio butonunun de�erini al
            var reservationID = selectedRadio.val();

            // AJAX iste�i g�nder
            $.ajax({
                type: "POST",
                url: "/Reservation/DeleteReservation", // �stek yap�lacak controller ve action'� belirtin
                data: { reservationID: reservationID },
                success: function (result) {
                    // �stek ba�ar�l� oldu�unda yap�lacak i�lemler
                    console.log("Rezervasyon iptal edildi.");
                },
                error: function () {
                    // �stek hatas� oldu�unda yap�lacak i�lemler
                    console.error("Rezervasyon iptal edilemedi. Bir hata olu�tu.");
                }
            });
        } else {
            // Se�ili bir radio butonu yoksa kullan�c�ya uyar� ver
            alert("L�tfen bir rezervasyon se�in.");
        }
    });
});