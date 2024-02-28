document.addEventListener('DOMContentLoaded', function () {
    var textarea = document.getElementById('reviewPrg');
    var current = document.getElementById('current');
    var maximum = document.getElementById('maximum');
    var theCount = document.getElementById('the-count');

    // Güvenlik kontrolü: Belirtilen öðelerin varlýðýný kontrol et
    if (textarea && current && maximum && theCount) {
        textarea.addEventListener('input', function () {
            var characterCount = this.value.length;

            current.textContent = characterCount;

            // Renk deðiþiklikleri
            if (characterCount < 50) {
                current.style.color = '#00ff00';
            } else if (characterCount < 100) {
                current.style.color = '#bfff00';
            } else if (characterCount < 200) {
                current.style.color = '#ffff00';
            } else if (characterCount < 300) {
                current.style.color = '#ff8000';
            } else if (characterCount < 400) {
                current.style.color = '#ff0000';
            }

            // Karakter sýnýrýna ulaþýldýðýnda stil deðiþiklikleri
            if (characterCount >= 400) {
                maximum.style.color = '#ff0000';
                current.style.color = '#ff0000';
                theCount.style.fontWeight = 'bold';
            } else {
                maximum.style.color = '#00ff00';
                theCount.style.fontWeight = 'normal';
            }
        });
    } else {
        console.error('Gerekli HTML öðeleri bulunamadý.');
    }
});