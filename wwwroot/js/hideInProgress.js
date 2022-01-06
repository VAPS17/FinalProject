var btnInProgress = document.querySelector('#InProgressContentToggle');
var contentInProgress = document.querySelector('.inProgress');

btnInProgress.addEventListener('click', function () {
    if (contentInProgress.style.display === 'block') {
        contentInProgress.style.display = 'none';
    } else {
        contentInProgress.style.display = 'block';
    }
})