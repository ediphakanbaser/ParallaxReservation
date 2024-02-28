document.getElementById('serv_img').addEventListener('change', function () {
    previewImage(this, 'previewService', 150, 100); // İstenen genişlik ve yükseklik değerlerini kullanabilirsiniz.
});

function previewImage(input, previewId, maxWidth, maxHeight) {
    const preview = document.getElementById(previewId);

    if (input.files && input.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const image = new Image();
            image.src = e.target.result;

            image.onload = function () {
                const canvas = document.createElement('canvas');
                const ctx = canvas.getContext('2d');

                let newWidth, newHeight;

                if (image.width > image.height) {
                    newWidth = maxWidth;
                    newHeight = (maxWidth / image.width) * image.height;
                } else {
                    newHeight = maxHeight;
                    newWidth = (maxHeight / image.height) * image.width;
                }

                canvas.width = newWidth;
                canvas.height = newHeight;

                ctx.drawImage(image, 0, 0, newWidth, newHeight);

                // Yeni eklenen kısım: canvas'ı bir div içine yerleştirme
                const canvasContainer = document.createElement('div');

                canvasContainer.appendChild(canvas);

                // Önceki önizlemeyi temizle
                preview.innerHTML = '';

                // Yeni önizlemeyi ekleyin
                preview.appendChild(canvasContainer);
            };
        };

        reader.readAsDataURL(input.files[0]);
    }
}