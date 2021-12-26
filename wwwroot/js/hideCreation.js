var btnCreation = document.querySelector('#CreationContentToggle');
var contentCreation = document.querySelector('.creation');

btnCreation.addEventListener('click', function () {
    if (contentCreation.style.display === 'block') {
        contentCreation.style.display = 'none';
    } else {
        contentCreation.style.display = 'block';
    }
})