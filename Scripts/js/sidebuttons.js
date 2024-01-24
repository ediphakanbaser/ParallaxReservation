var movebutton = document.querySelector(".move-button");
var wpbutton = document.querySelector(".wp-button");
window.addEventListener("scroll", function () {

    if (window.scrollY >= window.innerHeight) {
        wpbutton.classList.add("show-wp");
    } else {
        wpbutton.classList.remove("show-wp");
    }
});

window.addEventListener("scroll", function () {
    
    if (window.scrollY >= window.innerHeight * 2) {
        movebutton.classList.add("show-top");
    } else {
        movebutton.classList.remove("show-top");
    }
});

let scrollInterval;

movebutton.addEventListener("click", function () {
    if (window.scrollY >= window.innerHeight) {
        let scrollInterval = setInterval(function () {
            let difference = window.scrollY - window.innerHeight;
            let change = difference / 75;
            if (window.scrollY - 1 >= window.innerHeight) {
                window.scrollBy(0, -change); // Scroll deðerini her birimde 'change' kadar azalt
            } else {
                clearInterval(scrollInterval); // Belirtilen koþulu saðladýðýnda interval'i durdur
            }
        }, 1);
    }
});

