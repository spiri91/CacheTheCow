using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomObjs;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pipe4Net;
using Units.mocks;

[TestClass]
public class PuppeteerUnitTests
{
    List<MovieDto> mockedMovies = new List<MovieDto>()
        {
            new MovieDto(){id = "1", headline = "headline1", body = "randomtext1"},
            new MovieDto(){id = "2", headline = "headline2", body = "randomtext1"},
            new MovieDto(){id = "3", headline = "headline3", body = "randomtext1"},
        };

    [TestMethod]
    public void ShouldCreatePuppeteer()
    {
        var sut = Puppeteer.CreatePuppeteer(new MockedHttpService(), new MockedHandleHttpResultService(), "foo");
    }

    [TestMethod]
    public void ShouldNotCreatePuppeter()
    {
        Assert.ThrowsException<ArgumentException>(() => Puppeteer.CreatePuppeteer(null, null, null));
        Assert.ThrowsException<ArgumentException>(() => Puppeteer.CreatePuppeteer(null, new MockedHandleHttpResultService(), string.Empty));
        Assert.ThrowsException<ArgumentException>(() => Puppeteer.CreatePuppeteer(new MockedHttpService(), null, null));
    }

    [TestMethod]
    public void ShouldReturnIPuppeteer()
    {
        IPuppeteer sut = Puppeteer.CreatePuppeteer(new MockedHttpService(), new MockedHandleHttpResultService(), "foo");
    }

    [TestMethod]
    public void ShouldReturnIGetRepository()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHandleResultService.HandleFunc = (response, o) => new List<MovieDto>();

        IGetRepository sut = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result;
    }

    [TestMethod]
    public void ShoudReturnIRepository()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHandleResultService.HandleFunc = (response, o) => new List<MovieDto>();

        IRepository sut = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result.GetRepository();
    }

    [TestMethod]
    public void ShouldNotReturnIGetRepository()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHandleResultService.HandleFunc = (response, o) => new List<MovieDto>();

        Assert.ThrowsException<ArgumentException>(() => Puppeteer
            .CreatePuppeteer(mockedHttpService, mockedHandleResultService, null)
            .GetAllMovieList().Result);

        Assert.ThrowsException<ArgumentException>(() => Puppeteer
            .CreatePuppeteer(mockedHttpService, mockedHandleResultService, string.Empty)
            .GetAllMovieList().Result);
    }

    [TestMethod]
    public void ShouldReturnTheFirstMovieInTheList()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var repository = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository();

        var firstId = repository.TryGetFirstMovie().Result.id;

        var firstIdSecondTry = repository.TryGetFirstMovie().Result.id;

        Assert.IsTrue(mockedMovies.Exists(x => x.id == firstId));
        Assert.IsTrue(mockedMovies.Exists(x => x.id == firstIdSecondTry));

        Assert.IsTrue(firstId == firstIdSecondTry);
    }

    [TestMethod]
    public void ShouldReturnEmptyMovieOnEmptyListOfMovies()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => new List<MovieDto>();

        var firstMovie = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository().TryGetFirstMovie().Result;

        Assert.IsTrue(firstMovie.GetType().Name == nameof(EmptyMovie));
    }

    [TestMethod]
    public void ShouldReturnTheNumberOfAvailableMovies()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var count = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository().NumberOfMovieInTheList();

        Assert.IsTrue(count == mockedMovies.Count);
    }

    [TestMethod]
    public void ShouldReturnAllTheAvailableMovies()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var repository = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository();

        var availableMoviesReturnerd = repository.GetAllAvailableMovies();

        Assert.IsTrue(availableMoviesReturnerd.Count == mockedMovies.Count);

        availableMoviesReturnerd.ForEach(x => Assert.IsTrue(mockedMovies.Exists(y => y.id == x.id)));
    }

    [TestMethod]
    public void ShouldReturnEmptyListOnNoAvailableMovies()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => new List<MovieDto>();

        var repository = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository();

        var availableMoviesReturned = repository.GetAllAvailableMovies();

        Assert.IsTrue(availableMoviesReturned.Count == 0);
    }

    [TestMethod]
    public void ShouldReturnEmtpyMovieIfIdIsNotFoundAndTheListOfMoviesIsEmpty()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => new List<MovieDto>();

        var returnedMovie = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository().GetMovie("999").Result;

        Assert.IsTrue(returnedMovie.GetType().Name == nameof(EmptyMovie));
    }

    [TestMethod]
    public void ShouldReturnEmptyMovieIfIdIsNotFound()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var notExistingMovieId = mockedMovies.Select(x => new { _id = int.Parse(x.id) }).OrderByDescending(x => x._id)
                                   .First()._id + 1.ToString();

        var returnedMovie = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository().GetMovie(notExistingMovieId).Result;

        Assert.IsTrue(returnedMovie.GetType().Name == nameof(EmptyMovie));
    }

    [TestMethod]
    public void ShouldReturnMovieBasedOnId()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var repo = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository();

        var desiredMovie = mockedMovies.First();

        var movie = repo.GetMovie(desiredMovie.id).Result;

        Assert.IsTrue(movie.id == desiredMovie.id && movie.body == desiredMovie.body && movie.headline == desiredMovie.headline);
    }

    [TestMethod]
    public void ShouldReturnEmptyMovieIfNullIdIsGiven()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var returnedMovie = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository().GetMovie(null).Result;

        Assert.IsTrue(returnedMovie.GetType().Name == nameof(EmptyMovie));
    }

    [TestMethod]
    public void ShouldReturnEmptyMovieIfEmptyIdIsGiven()
    {
        var mockedHttpService = new MockedHttpService();
        var mockedHandleResultService = new MockedHandleHttpResultService();

        mockedHttpService.GetAsyncFunc = s => new HttpResponse(true, "foo", "bar");
        mockedHttpService.GetImageAsBase64Func = s => "bar";

        mockedHandleResultService.HandleFunc = (response, o) => mockedMovies;

        var returnedMovie = Puppeteer.CreatePuppeteer(mockedHttpService, mockedHandleResultService, "foo")
            .GetAllMovieList().Result
            .GetRepository().GetMovie(string.Empty).Result;

        Assert.IsTrue(returnedMovie.GetType().Name == nameof(EmptyMovie));
    }
}
