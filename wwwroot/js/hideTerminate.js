var btnTerminate = document.querySelector('#TerminateContentToggle');
var contentTerminate = document.querySelector('.terminate');

btnTerminate.addEventListener('click', function () {
    if (contentTerminate.style.display === 'block') {
        contentTerminate.style.display = 'none';
    } else {
        contentTerminate.style.display = 'block';
    }
})