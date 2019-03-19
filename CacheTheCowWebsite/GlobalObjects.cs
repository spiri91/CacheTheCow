using System;

namespace CacheTheCowWebsite
{
    public static class GlobalObjects
    {
        public static IRepository MovieRepository { get; private set; }

        public static void SetMovieRepository(IRepository repository)
        {
            MovieRepository = repository ?? throw new ArgumentException();
        }
    }
}
