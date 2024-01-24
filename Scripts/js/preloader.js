window.addEventListener("load", () => {
    const loader = document.querySelector(".loader");
    document.body.style.overflow = 'hidden';
    setTimeout(() => {
        loader.classList.add("loader--hidden");
        document.body.style.overflow = 'visible';
        loader.addEventListener("transitionend", () => {
            document.body.removeChild(loader);
            
        });
    }, 1000);
});
