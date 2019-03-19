var templateMediaInfo = `
   <div class='container'>
    <h5>Description</h5><br>
    <div class='row'>
        <div class='col-sm-12'>
            <p>{{body}}<p>
        </div>
        <div class='col-sm-12'>
            <span>Cast: <span><p>{{cast}}</p>
        </div>
        <div class='col-sm-12'>
            <span>Cert: <span><p>{{cert}}</p>
        </div>
        <div class='col-sm-12'>
            <span>Class: <span><p>{{class}}</p>
        </div>
        <div class='col-sm-12'>
            <span>Directors: <span><p>{{directors}}</p>
        </div>
        <div class='col-sm-12'>
            <span>headline: <span><p>{{headline}}</p>
        </div>
        <div class='col-sm-12'>
            <span>Duration: <span><p>{{duration}}</p>
        </div>
         <div class='col-sm-12'>
            <span>Genres: <span><p>{{genres}}</p>
        </div>
         <div class='col-sm-12'>
            <span>id: <span><p>{{id}}</p>
        </div>
        <div class='col-sm-12'>
            <span>last updated: <span><p>{{lastUpdated}}</p>
        </div>
        <div class='col-sm-12'>
            <span>quote: <span><p>{{quote}}</p>
        </div>
        <div class='col-sm-12'>
            <span>rating: <span><p>{{rating}}</p>
        </div>
        <div class='col-sm-12'>
            <span>review author: <span><p>{{reviewAuthor}}</p>
        </div>
        <div class='col-sm-12'>
            <span>sum: <span><p>{{sum}}</p>
        </div>
        <div class='col-sm-12'>
            <span>skyGoId: <span><p>{{skyGoId}}</p>
        </div>
        <div class='col-sm-12'>
            <span>skyGoUrl: <span><p>{{skyGoUrl}}</p>
        </div>
        <div class='col-sm-12'>
            <span>synopsis: <span><p>{{synopsis}}</p>
        </div>
        <div class='col-sm-12'>
            <span>url: <span><p>{{url}}</p>
        </div>
        <div class='col-sm-12'>
            <span>year: <span><p>{{year}}</p>
        </div>
    <div>
   <div>   
`;

let templateViewingWindow = `
    <div class='container'>
        <h5>Viewing Window: </h5>
        <div class='row'>
            <div class='col-sm-12'>
                <span>start date: <span><p>{{startDate}}<p>
            </div>
            <div class='col-sm-12'>
                <span>way to watch: <span><p>{{wayToWatch}}<p>
            </div>
            <div class='col-sm-12'>
                <span>end date: <span><p>{{endDate}}<p>
            </div>
        </div>
    </div>

`;

let videosTemplate = `
    <div class='container'>
      <h5>Videos: </h5>
        
        {{#.}}
            <h6>Video: <h6>
            <div class='row'>
                <div class='col-sm-12'>
                    <span>Title: </span> <p>{{title}}</p>
                    <span>Type: </span> <p>{{type}}</p>
                    <a href='{{url}}'>Link</a><br><br>
                    <span>Alternatives</span><br>
                    {{#alternatives}}
                        <a href='{{url}}'>{{quality}}</a>&nbsp 
                    {{/alternatives}}
                </div>
            </div> 
            <br>
        {{/.}}
    </div>
`;

let imagesTemplate = `
    <div class='container'>
        <h5>Images: </h5>
             <div class='row'>
                {{#.}}
                   <div class='col-sm-3'>
                        <image style='height: 130px; width: 80px;' src='data:{{base64}}'/>
                    </div>
                {{/.}}
            </div>
        <div>
    </div>
`;

let keyArtImagesTemplate = `
    <div class='container'>
        <h5>Key art images: </h5>
             <div class='row'>
                {{#.}}
                   <div class='col-sm-3'>
                        <image style='height: 130px; width: 80px;' src='data:{{base64}}'/>
                    </div>
                {{/.}}
            </div>
        <div>
    </div>
`;


function templating() {
    function concat(array, separator, property) {
        let temp = '';

        for (let i in array) {
            temp += property ? array[i][property] : array[i];

            if (i != array.length - 1) temp += separator;

            temp += " ";
        }

        return temp;
    }

    function preprocess(movie) {
        movie.cast = concat(movie.cast, ',', "name");
        movie.directors = concat(movie.directors, ',', "name");
        movie.genres = concat(movie.genres, ',');
    }

    function renderMediaInfo(movie) {
        let res = Mustache.render(templateMediaInfo, movie);
        $('.movieInfo')[0].innerHTML = res;
    }

    function renderViewingWindow(viewingWindow) {
        let res = Mustache.render(templateViewingWindow, viewingWindow);
        $('.viewingWindow')[0].innerHTML = res;
    }

    function  renderVideos(videos) {
        let res = Mustache.render(videosTemplate, videos);
        $('.videos')[0].innerHTML = res;
    }

    function renderImages(images) {
        let res = Mustache.render(imagesTemplate, images);
        $('.images')[0].innerHTML = res;
    }

    function renderKeyArtImages(images) {
        let res = Mustache.render(keyArtImagesTemplate, images);
        $('.keyArtImages')[0].innerHTML = res;
    }

    function showMovie(movie) {
        preprocess(movie);

        renderMediaInfo(movie);
        movie.viewingWindow && renderViewingWindow(movie.viewingWindow);
        movie.videos && renderVideos(movie.videos);
        movie.cardImages && renderImages(movie.cardImages);
        movie.keyArtImages && renderKeyArtImages(movie.keyArtImages);
    }

    return {
        showMovie: showMovie
    }
}