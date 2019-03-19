using Acceptance.wrappers;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Obligations;

[TestClass]
public class PuppeteerAcceptanceTests
{
    string urlForMovieList = "https://mgtechtest.blob.core.windows.net/files/showcase.json";

    /// <summary>
    /// Given an url that returns a json array of objects,
    /// I cache the result and never call that url again.
    ///
    /// == Long running test!!! =
    /// 
    /// </summary>
    [TestMethod]
    public void UrlIsCalledOnlyOnce()
    {
        // Arange
        var httpServiceWrapper = new HttpServiceWrapper();
        var sut = Puppeteer.CreatePuppeteer(httpServiceWrapper, new Dealer().GiveMeA<IHandleResponse>(), urlForMovieList, false)
            .GetAllMovieList().Result.GetRepository();
        var movie = sut.TryGetFirstMovie().Result;

        int iterations = sut.NumberOfMovieInTheList();
        int currentIteration = 0;

        // Act
        while (currentIteration < iterations)
        {
            movie = sut.GetMovie(movie.nextMovie).Result;
            currentIteration++;
        }

        // Assert
        Assert.IsTrue(httpServiceWrapper.numberOfTimesGetOrGetAsyncIsCalled == 1);
    }


    /// <summary>
    /// Given a movie in json format,
    /// I cache the card images and key art images on the first access as base64
    /// 
    /// == Long running test!!! ==
    /// 
    /// </summary>
    [TestMethod]
    public void GivenAMovieAsJsonICacheTheImagesOnFirstAccess()
    {
        // Arange
        var httpServiceWrapper = new HttpServiceWrapper();
        var sut = Puppeteer.CreatePuppeteer(httpServiceWrapper, new Dealer().GiveMeA<IHandleResponse>(), urlForMovieList, false)
            .GetAllMovieList().Result.GetRepository();

        var movie = sut.TryGetFirstMovie().Result;

        var numberOfTimesWeShouldTryGetAMovie = 3;

        // Act 
        var numberOfTimesWeShouldCallTheImagesUrl = movie.cardImages.Count + movie.keyArtImages.Count;

        var counter = 0;

        while (counter < numberOfTimesWeShouldTryGetAMovie)
        {
            var _ = sut.GetMovie(movie.id).Result;

            counter++;
        }

        // Assert
        Assert.IsTrue(numberOfTimesWeShouldCallTheImagesUrl == httpServiceWrapper.numberOfTimesTheImageUrlsAreCalled);
    }

    /// <summary>
    /// Traversing all the available movies,
    /// When trying to retrieve each one multiple times,
    /// I should only call the urls for each movie image just once
    ///
    /// == Long running test!!! ==
    /// 
    /// </summary>
    [TestMethod]
    public void TraversingAllTheMoviesWillCallEachImageUrlOnlyOnce()
    {
        // Arange 
        var httpServiceWrapper = new HttpServiceWrapper();
        IRepository sut = Puppeteer.CreatePuppeteer(httpServiceWrapper, new Dealer().GiveMeA<IHandleResponse>(), urlForMovieList, false)
            .GetAllMovieList().Result.GetRepository();

        // Act && Assert
        var availableMovies = sut.GetAllAvailableMovies();

        foreach (var movie in availableMovies)
        {
            httpServiceWrapper.ResetNumberOfTimesTheImageUrlsAreCalled();

            int contor = 0;

            MovieDto movieDto = new MovieDto();

            while (contor < 3)
            {
                movieDto = sut.GetMovie(movie.id).Result;
                contor++;
            }

            Assert.IsTrue(httpServiceWrapper.numberOfTimesTheImageUrlsAreCalled == movieDto.cardImages.Count + movieDto.keyArtImages.Count);
        }
    }
}
