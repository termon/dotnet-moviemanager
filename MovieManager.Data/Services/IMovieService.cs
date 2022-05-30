
using System.Collections.Generic;
using MovieManager.Core.Models;

namespace MovieManager.Data.Services
{
    public interface IMovieService
    {
        void Initialise();

        IList<Movie> GetAllMovies(string orderBy=null);        
        IList<Movie> GetMoviesByActor(string name);
        Movie GetMovieById(int id);
        bool DeleteMovie(int id);
        bool UpdateMovie(Movie m);
        Movie AddMovie(Movie m);
        Review GetReviewById(int id);
        Review AddReview(Review r);
        bool DeleteReview(int id);
        
    }
}