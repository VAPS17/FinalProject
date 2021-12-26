var btnInitial = document.querySelector('#InitialContentToggle');
var contentInitial = document.querySelector('.initialize');

btnInitial.addEventListener('click', function () {
    if (contentInitial.style.display === 'block') {
        contentInitial.style.display = 'none';
    } else {
        contentInitial.style.display = 'block';
    }
})