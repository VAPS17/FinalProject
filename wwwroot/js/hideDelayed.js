var btnDelayed = document.querySelector('#DelayedContentToggle');
var contentDelayed = document.querySelector('.delayed');

btnDelayed.addEventListener('click', function () {
    if (contentDelayed.style.display === 'block') {
        contentDelayed.style.display = 'none';
    } else {
        contentDelayed.style.display = 'block';
    }
})