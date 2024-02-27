function openEditModal(employeeID) {
    // Burada AJAX veya baþka bir yöntemle employeeID'ye baðlý olarak çalýþan bilgilerini alabilirsiniz.
    // Örnek olarak sadece modalý görünür yapalým:
    var modal = document.getElementById('editModal');
    modal.style.display = 'flex';

    // Örnek: AJAX kullanarak çalýþan bilgilerini almak
    $.ajax({
        url: '/Employee/GetEmployeeDetails',
        type: 'GET',
        data: { employeeID: employeeID },
        success: function (data) {
            // Burada AJAX isteði baþarýlý olduðunda çalýþan bilgilerini modal içine yerleþtirme iþlemini yapabilirsiniz.
            // Örnek olarak sadece console.log ile gösteriyoruz:
            console.log(data);
        },
        error: function (error) {
            console.error('Çalýþan bilgileri alýnamadý:', error);
        }
    });
}

// Düzenleme formunu kapatan fonksiyon
function closeEditModal() {
    var modal = document.getElementById('editModal');
    modal.style.display = 'none';
}