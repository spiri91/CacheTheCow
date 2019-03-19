using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataObjects
{
    public sealed class Image
    {
        public string url;
        public double h;
        public double w;
        public string base64;
    }

    public sealed class People
    {
        public string name;
    }

    public sealed class Alternatives
    {
        public string quality;

        public string url;
    }

    public sealed class Videos
    {
        public string title;

        public List<Alternatives> alternatives = new List<Alternatives>();

        public string type;

        public string url;
    }

    public sealed class ViewingWindow
    {
        public string title;

        public string startDate;

        public string wayToWatch;

        public string endDate;
    }

    public class Movie
    {
        public string id;

        public string headline;
    }

    public class MovieDto : Movie
    {
        public string body;

        public List<Image> cardImages = new List<Image>();

        public List<People> cast = new List<People>();

        public string cert;

        public string @class;

        public List<People> directors = new List<People>();

        public double duration;

        public List<string> genres = new List<string>();

        public List<Image> keyArtImages = new List<Image>();

        public string lastUpdated;

        public string quote;

        public double rating;

        public string reviewAuthor;

        public string skyGoId;

        public string skyGoUrl;

        public string sum;

        public string synopsis;

        public string url;

        public List<Videos> videos = new List<Videos>();

        public ViewingWindow viewingWindow;

        public int year;

        public string previousMovie = string.Empty;

        public string nextMovie = string.Empty;
    }

    public class EmptyMovie : MovieDto { }
}
