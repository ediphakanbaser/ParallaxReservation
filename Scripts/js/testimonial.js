document.addEventListener('DOMContentLoaded', function () {
    var titleElement = document.getElementById('testimonial-title');
    var absoluteBoxElement = document.querySelector('.absolute-box');
    var testimonialConElement = document.querySelector('.testimonial-con');
    var indicators = document.querySelector('.comment-indicators');
    var slider = document.querySelector('.slider-con');
    var img = document.querySelector('.customer-image');


    // Sayfanýn body baþlangýcýndan testimonial-title baþlangýcýna kadar olan uzaklýðý hesapla
    var distanceToTop = titleElement.getBoundingClientRect().top;

    // 7.5vh ekleyerek yeni top pozisyonunu hesapla
    var newTopPosition = distanceToTop + window.scrollY + window.innerHeight * 0.125 + 125;


    // Absolute-box elementini yeni top pozisyonuna yerleþtir

    indicators.style.top = newTopPosition + 'px';
    testimonialConElement.style.top = newTopPosition + 'px';

    // Absolute-box elementinin boyutunu al
    var boxDimensions = absoluteBoxElement.getBoundingClientRect();

    // Testimonial-con elementinin mevcut boyutunu al
    var testimonialConDimensions = testimonialConElement.getBoundingClientRect();

    // Yeni geniþlik ve yükseklik deðerlerini hesapla
    var newHeight = testimonialConDimensions.height + boxDimensions.height;

    // Testimonial-con elementinin boyutunu güncelle
    
    slider.style.height = newHeight + 'px';
    testimonialConElement.style.height = newHeight + 'px';
    
});

document.addEventListener('DOMContentLoaded', function () {
    var absoluteBox = document.querySelector('.absolute-box');
    var beforeButton = document.getElementById('before');
    var afterButton = document.getElementById('after');
    var commentBoxes = document.querySelectorAll('.comment-box');

    var slideIndex = 0;
    var totalSlides = commentBoxes.length;

    beforeButton.addEventListener('click', function () {
        slideIndex--;
        if (slideIndex < 0) {
            slideIndex = totalSlides - 1;
        }
        showSlide();
    });

    afterButton.addEventListener('click', function () {
        slideIndex++;
        if (slideIndex >= totalSlides) {
            slideIndex = 0;
        }
        showSlide();
    });

    function showSlide() {
        var slideWidth = commentBoxes[0].clientWidth;

        // Her butona týklandýðýnda sadece comment box'larý kaydýr
        var newTransformValue = -slideIndex * slideWidth;
        for (var i = 0; i < commentBoxes.length; i++) {
            commentBoxes[i].style.transform = 'translateX(' + newTransformValue + 'px)';
        }
    }
});
