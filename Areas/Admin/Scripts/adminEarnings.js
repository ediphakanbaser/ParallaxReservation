document.addEventListener('DOMContentLoaded', function () {
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();
    var previousYear = currentYear - 1;
    var nextYear = currentYear + 1;

    document.getElementById("currentYearAccordion").innerHTML = currentYear;
    $("#currentYearAccordion").val(currentYear);
    document.getElementById("previousYearAccordion").innerHTML = previousYear;
    $("#previousYearAccordion").val(previousYear);
    document.getElementById("nextYearAccordion").innerHTML = nextYear;
    $("#nextYearAccordion").val(nextYear);

    document.getElementById("currentYearAccordion").addEventListener("click", function () {
        togglePanel("currentYearPanel", currentYear);
    });

    document.getElementById("previousYearAccordion").addEventListener("click", function () {
        togglePanel("previousYearPanel", previousYear);
    });

    document.getElementById("nextYearAccordion").addEventListener("click", function () {
        togglePanel("nextYearPanel", nextYear);
    });

    function togglePanel(panelId, year) {
        var panel = document.getElementById(panelId);
        var panelContent = "";

        var months = ["Ocak", "�ubat", "Mart", "Nisan", "May�s", "Haziran", "Temmuz", "A�ustos", "Eyl�l", "Ekim", "Kas�m", "Aral�k"];

        months.forEach(function (month, index) {
            panelContent += "<button class='month' value='" + (index + 1) + "' data-year='" + year + "'>" + month + "</button>";
        });

        panel.innerHTML = panelContent;
        panel.style.display = (panel.style.display === "block") ? "none" : "block";

        var monthButtons = panel.getElementsByClassName("month");
        for (var i = 0; i < monthButtons.length; i++) {
            monthButtons[i].addEventListener("click", function () {
                var selectedMonth = $(this).val();
                var selectedYear = $(this).data("year");
                $(".yearMonth").text(selectedMonth + "-" + selectedYear);
                getData(selectedYear, selectedMonth);
            });
        }
    }
});
function getData(selectedYear, selectedMonth) {
    $.ajax({
        url: '/Dashboard/GetGraphInfo', // ControllerName, MVC controller'�n�z�n ad�yla de�i�tirilmelidir
        type: 'POST',
        dataType: 'json',
        data: { selectedYear: selectedYear, selectedMonth: selectedMonth },
        success: function (data) {
            // AJAX iste�i ba�ar�l� oldu�unda buraya gelecek olan i�lemler
            console.log(data); // Gelen verileri konsola yazd�rabilirsiniz
            var labels = [];
            var totalFees = [];
            var dailyTotalSum = 0;

            // Veriyi i�leyerek gerekli formatlara d�n��t�rme
            for (var i = 0; i < data.length; i++) {
                labels.push('Day ' + data[i].Day);
                dailyTotalSum += data[i].TotalFee;
                totalFees.push(data[i].TotalFee);
            }
            $(".totalFee").text(dailyTotalSum);
            // Canvas elementini se�me
            var ctx = document.getElementById('myChart').getContext('2d');

            var existingChart = Chart.getChart(ctx);
            if (existingChart) {
                existingChart.destroy();
            }

            // Chart.js kullanarak �izgi grafi�i olu�turma
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Total Fee',
                        data: totalFees,
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                }
            });
        },
        error: function (error) {
            // AJAX iste�i ba�ar�s�z oldu�unda buraya gelecek olan i�lemler
            console.log('Hata olu�tu: ' + error);
        }
    });
}