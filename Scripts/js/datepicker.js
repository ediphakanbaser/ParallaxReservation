$("#datepicker").datepicker({

      dateFormat: 'yy-mm-dd',
      beforeShowDay: function(date) {
        var day = date.getDay();
        // Pazar günlerini seçilemez yap
        return [(day !== 0), ''];
      },
      onSelect: function(dateText, inst) {

          $("#datepicker-input").val(dateText);
      },
      firstDay: 1 // Pazartesi gününü ilk gün olarak ayarla
    });
  
    

document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
      initialView: 'dayGridMonth', // Takvimin ilk açıldığında hangi görünümde olacağı
      headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay'
      },
      events: [
        // Takvimde gösterilecek etkinlikler burada tanımlanabilir (isteğe bağlı)
        // { title: 'Etkinlik Başlığı', start: '2023-11-25' }
      ]
    });

    calendar.render();
  });
  