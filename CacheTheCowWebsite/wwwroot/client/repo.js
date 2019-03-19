function repo() {
    const apiAddress = location.href + "api/movies";

    function getMovie(id) {
        return $.get(apiAddress + "/" + id);
    }

    function getFirstMovie() {
        return $.get(apiAddress);
    }

    return {
        getMovie: getMovie,
        getFirstMovie: getFirstMovie
    }
}