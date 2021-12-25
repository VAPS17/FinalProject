var btnComent = document.querySelector('#contentToggle');
var content = document.querySelector('.comentary-card');

btnComent.addEventListener('click', function () {
    if (content.style.display === 'block') {
        content.style.display = 'none';
    } else {
        content.style.display = 'block';
    }
})