var btnInicialize = document.querySelector('#InicializeContentToggle');
var contentInicialize = document.querySelector('.inicialize');

btnInicialize.addEventListener('click', function () {
    if (contentInicialize.style.display === 'block') {
        contentInicialize.style.display = 'none';
    } else {
        contentInicialize.style.display = 'block';
    }
})