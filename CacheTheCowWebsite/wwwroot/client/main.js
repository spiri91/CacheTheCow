/// <reference path="repo.js" />
/// <reference path="templating.js" />

var repo = repo();
var templating = templating();

function showLoadingDiv() {
    $('#loaderDiv').show();
}

function hideLoadingDiv() {
    $('#loaderDiv').hide();
}

function setNextAndPreviousMovie(previousId, nextId) {
    let prevBtn = $('#previousMovie');

    let nextBtn = $('#nextMovie');

    nextBtn.unbind('click');
    prevBtn.unbind('click');

    previousId && prevBtn.click(() => {
        showLoadingDiv();
        repo.getMovie(previousId).then(showMovie).then(hideLoadingDiv);
    });
    nextId &&
        nextBtn.click(() => {
            showLoadingDiv();
            repo.getMovie(nextId).then(showMovie).then(hideLoadingDiv);
        });
};

function showMovie(movie) {
    templating.showMovie(movie);
    setNextAndPreviousMovie(movie.previousMovie, movie.nextMovie);
}

function init() {
    hideLoadingDiv();
    repo.getFirstMovie().then(showMovie);
};


$(document).ready(() => {
    init();
})