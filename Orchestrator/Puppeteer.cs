using System;
using System.Collections;
using CustomObjs;
using DataObjects;
using Obligations;
using Pipe4Net;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IPuppeteer
{
    Task<IGetRepository> GetAllMovieList();
};

public interface IGetRepository
{
    IRepository GetRepository();
}

public interface IRepository
{
    Task<MovieDto> GetMovie(string id);

    Task<MovieDto> TryGetFirstMovie();

    int NumberOfMovieInTheList();

    IList<Movie> GetAllAvailableMovies();
}

public class Puppeteer : IPuppeteer, IRepository, IGetRepository
{
    private IMakeHttpRequest httpRequestService;
    private IHandleResponse handleResponseService;
    private string url;
    private bool precacheMovies;

    private ConcurrentDictionary<string, MovieDto> moviesWithoutImagesCached;
    private ConcurrentDictionary<string, MovieDto> moviesWithImagesCached;
    private IList<Movie> availableMovies;

    /// <summary>
    /// == Use the static method CreatePuppeteer instead == 
    /// </summary>
    /// <param name="httpRequestService"></param>
    /// <param name="handleResponseService"></param> 
    private Puppeteer() { }

    private Puppeteer(IMakeHttpRequest httpRequestService, IHandleResponse handleResponseService, NotEmptyString url, bool prechacheMovies)
    {
        this.httpRequestService = httpRequestService;
        this.handleResponseService = handleResponseService;
        this.url = url;
        this.precacheMovies = prechacheMovies;

        moviesWithImagesCached = new ConcurrentDictionary<string, MovieDto>();
        moviesWithoutImagesCached = new ConcurrentDictionary<string, MovieDto>();
        availableMovies = new List<Movie>();
    }


    // public testable
    public static IPuppeteer CreatePuppeteer(IMakeHttpRequest httpRequestService, IHandleResponse handleResponseService, NotEmptyString url, bool precacheMovies = true)
    {
        if (null == httpRequestService || null == handleResponseService || null == url) throw new ArgumentException();

        return new Puppeteer(httpRequestService, handleResponseService, url, precacheMovies);
    }

    public async Task<IGetRepository> GetAllMovieList()
    {
        var movies = await GetAllMoviesInformations(url);

        if (movies.Count == 0) return this;

        StoreAvailableMovies(movies);
        StorePreviousAndNextInEachMovie(movies);
        StoreMoviesInHashTable(movies);

        if (precacheMovies) StartPrecachingMovies(movies);

        return this;
    }

    public IRepository GetRepository() => this;

    public async Task<MovieDto> TryGetFirstMovie()
    {
        if (moviesWithImagesCached.Count != 0) return moviesWithImagesCached.First().Value;

        if (moviesWithoutImagesCached.Count == 0) return new EmptyMovie();

        var movie = moviesWithoutImagesCached.First().Value;

        return CacheMovieAndReturnIt(movie);
    }

    public int NumberOfMovieInTheList() => moviesWithoutImagesCached.Count;

    public IList<Movie> GetAllAvailableMovies() => availableMovies;

    public async Task<MovieDto> GetMovie(string id)
    {
        if (String.IsNullOrEmpty(id)) return new EmptyMovie();

        MovieDto movie = TryGetMovieFromCachedMovies(id);

        if (null != movie) return movie;

        movie = TryGetMovieFromReturnedList(id);

        if (null == movie) return new EmptyMovie();

        return CacheMovieAndReturnIt(movie);
    }


    // private inner logic
    private void StoreAvailableMovies(IList<MovieDto> movies)
    {
        availableMovies = movies.Select(x => new Movie()
        {
            id = x.id,
            headline = x.headline
        }).ToList();
    }

    private MovieDto CacheMovieAndReturnIt(MovieDto movie)
    {
        EmbedImagesAsBase64For(movie);

        AddMovieToCachedMovies(movie);

        return movie;
    }

    private MovieDto TryGetMovieFromCachedMovies(NotEmptyString id)
    {
        moviesWithImagesCached.TryGetValue(id, out MovieDto movie);

        return movie;
    }

    private MovieDto TryGetMovieFromReturnedList(NotEmptyString id)
    {
        moviesWithoutImagesCached.TryGetValue(id, out MovieDto movie);

        return movie;
    }

    private void AddMovieToCachedMovies(MovieDto movie) =>
        moviesWithImagesCached.AddOrUpdate(movie.id, movie, (id, _movie) => movie);

    private async Task<IList<MovieDto>> GetAllMoviesInformations(NotEmptyString url)
    {
        var getRequestResult = await httpRequestService.GetAsync(url);
        IList<MovieDto> movieList = handleResponseService.HandleArrayResult<MovieDto>(getRequestResult, new MovieDto[0]);

        return movieList;
    }

    private void EmbedImagesAsBase64For(MovieDto movie)
    {
        movie.cardImages.ForEach(ci => ci.base64 = httpRequestService.GetImageAsBase64url(ci.url).Result);
        movie.keyArtImages.ForEach(kai => kai.base64 = httpRequestService.GetImageAsBase64url(kai.url).Result);
    }

    private void StorePreviousAndNextInEachMovie(IList<MovieDto> movies)
    {
        string previousId = movies[movies.Count - 1].id;

        for (var i = 0; i < movies.Count; i++)
        {
            movies[i].previousMovie = previousId;
            movies[i].nextMovie = i < movies.Count - 1 ? movies[i + 1].id : movies[0].id;

            previousId = movies[i].id;
        }
    }

    private void StoreMoviesInHashTable(IList<MovieDto> movies)
        => movies.ForEach(m => moviesWithoutImagesCached.AddOrUpdate(m.id, m, (id, movie) => movie));

    private void StartPrecachingMovies(IList<MovieDto> movies)
    {
        Task.Factory.StartNew(() =>
        {
            var iterator = movies.GetEnumerator();

            while (iterator.MoveNext())
            {
                GetMovie(iterator.Current.id);
            }

        }, TaskCreationOptions.LongRunning);
    }
}
