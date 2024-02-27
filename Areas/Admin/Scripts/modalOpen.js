function openEditModal(employeeID) {
    // Burada AJAX veya ba�ka bir y�ntemle employeeID'ye ba�l� olarak �al��an bilgilerini alabilirsiniz.
    // �rnek olarak sadece modal� g�r�n�r yapal�m:
    var modal = document.getElementById('editModal');
    modal.style.display = 'flex';

    // �rnek: AJAX kullanarak �al��an bilgilerini almak
    $.ajax({
        url: '/Employee/GetEmployeeDetails',
        type: 'GET',
        data: { employeeID: employeeID },
        success: function (data) {
            // Burada AJAX iste�i ba�ar�l� oldu�unda �al��an bilgilerini modal i�ine yerle�tirme i�lemini yapabilirsiniz.
            // �rnek olarak sadece console.log ile g�steriyoruz:
            console.log(data);
        },
        error: function (error) {
            console.error('�al��an bilgileri al�namad�:', error);
        }
    });
}

// D�zenleme formunu kapatan fonksiyon
function closeEditModal() {
    var modal = document.getElementById('editModal');
    modal.style.display = 'none';
}