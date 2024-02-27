const mesaiBaslangicInput = document.getElementById('mesaiBaslangic');
const molaBaslangicInput = document.getElementById('molaBaslangic');
const molaBitisInput = document.getElementById('molaBitis');
const mesaiBitisInput = document.getElementById('mesaiBitis');

// Mesai baþlangýcý ve bitiþi için zaman aralýðýný belirle
const baslangicSaat = '00:00';
const bitisSaat = '23:59';

// Her bir time input elemanýna zaman aralýðýný uygula
mesaiBaslangicInput.setAttribute('min', baslangicSaat);
mesaiBaslangicInput.setAttribute('max', bitisSaat);

molaBaslangicInput.setAttribute('min', baslangicSaat);
molaBaslangicInput.setAttribute('max', bitisSaat);

molaBitisInput.setAttribute('min', baslangicSaat);
molaBitisInput.setAttribute('max', bitisSaat);

mesaiBitisInput.setAttribute('min', baslangicSaat);
mesaiBitisInput.setAttribute('max', bitisSaat);