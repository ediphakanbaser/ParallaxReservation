document.getElementById('emp_img').addEventListener('change', function () {
    previewImage(this, 'previewEmployee', 40, 30); // Ýstenen geniþlik ve yükseklik deðerlerini kullanabilirsiniz.
});

document.getElementById('update_img').addEventListener('change', function () {
    previewImage(this, 'previewUpdate', 40, 30); // Ýstenen geniþlik ve yükseklik deðerlerini kullanabilirsiniz.
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

                // Yeni eklenen kýsým: canvas'ý bir div içine yerleþtirme
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