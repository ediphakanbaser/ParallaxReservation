const slides = document.querySelectorAll('.slide');
const next = document.getElementById('next');
const prev = document.getElementById('prev');
let activeIndex = 0;

const nextSlide = () => {
    
    slides[activeIndex].classList.remove('active');
    activeIndex = (activeIndex + 1) % slides.length; 
    slides[activeIndex].classList.add('active');
    
}

const prevSlide = () => {
    slides[activeIndex].classList.remove('active');
    activeIndex = (activeIndex - 1 + slides.length) % slides.length;
    slides[activeIndex].classList.add('active');
}

next.addEventListener('click', ()=> {
    nextSlide();
});

prev.addEventListener('click', ()=> {
    prevSlide();
});
