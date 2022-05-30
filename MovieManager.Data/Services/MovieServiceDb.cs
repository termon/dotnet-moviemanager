using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using MovieManager.Core.Models;
using MovieManager.Data.Repositories;

namespace MovieManager.Data.Services
{
    public class MovieServiceDb : IMovieService
    {
        private readonly DatabaseContext ctx;

        public MovieServiceDb()
        {           
            ctx = DatabaseContextFactory.CreateCtx();
        }

        public MovieServiceDb(DatabaseContext _ctx)
        {
            ctx = _ctx;
        }

        /// <summary>
        /// Used to initialise the database - use with care
        /// </summary>
        public void Initialise()
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }

        /// <summary>
        /// Return a list of movies optionally ordered by title, director or year
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IList<Movie> GetAllMovies(string orderBy)
        {            
            switch (orderBy)
            {
                case "title" :
                    return ctx.Movies.OrderBy(m => m.Title).ToList(); 
                case "director":                
                    return ctx.Movies.OrderBy(m => m.Year).ToList();
                case "year" :
                    return ctx.Movies.OrderBy(m => m.Year).ToList();
                case "genre" :
                    return ctx.Movies.OrderBy(m => m.Genre).ToList();
                default:
                    return ctx.Movies.OrderBy(m => m.Id).ToList(); 
            }
        }

        /// <summary>
        /// Return list of movies where the cast contains the actor name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<Movie> GetMoviesByActor(string name)
        {
            return ctx.Movies.Include(m => m.Reviews).Where(m => m.Cast.Contains(name)).ToList();
        }
     
        /// <summary>
        /// Return the movie identified by id  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Movie or null if not found</returns>
        public Movie GetMovieById(int id)
        {            
            return ctx.Movies
                .Include(ms => ms.Reviews)
                .FirstOrDefault(m => m.Id == id);
        }
  
        /// <summary>
        /// Add to specified movie
        /// </summary>
        /// <param name="m"></param>
        /// <returns>The added movie if successful otherwise null</returns>
        public Movie AddMovie(Movie m)
        {
            ctx.Movies.Add(m);
            ctx.SaveChanges();
            return m;
        }

        /// <summary>
        /// Update the specified movie
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns true if update successful otherwise false</returns>
        public bool UpdateMovie(Movie m)
        {
            // verify that this student exists
            var o = GetMovieById(m.Id);
            if (o == null)
            {
                return false;
            }

            // ** disconnect entity (o) from EF Change tracking so we **
            // ** can update the new entity (s) without a conflict    **
            ctx.Entry(o).State = EntityState.Detached;

            // tell EF that this entity has changed
            ctx.Update(m);

            //ctx.Attach(m).State = EntityState.Modified;

            ctx.SaveChanges();
            return true;
        }

        
        /// <summary>
        /// Delete the specified movie
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns true if deletion successful otherwise false</returns>
        public bool DeleteMovie(int id)
        {
            var m = GetMovieById(id);
            if (m == null)
            {
                return false;
            }

            ctx.Movies.Remove(m);
            ctx.SaveChanges();
            return true;
        }
        
        
        /// <summary>
        /// Return the specified Review
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns the Review if found otherwise false</returns>
        public Review GetReviewById(int id)
        {     
            return ctx.Reviews
                .Include(r => r.Movie)
                .FirstOrDefault(r => r.Id == id);            
        }

        /// <summary>
        /// Add the specified Review
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns the added Review if successful otherwise null</returns>
        public Review AddReview(Review r)
        {
            var movie = GetMovieById(r.MovieId);
            if (movie == null)
            {
                return null;
            }

            movie.Reviews.Add(r);
            ctx.SaveChanges();
            return r;
        }
        
        /// <summary>
        /// Delete the specified Review
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns true if deletion successful otherwise false</returns>
        public bool DeleteReview(int Id)
        {
            var review = ctx.Reviews.FirstOrDefault((r => r.Id == Id));
            if (review == null)
            {
                return false;
            }
                  
            ctx.Remove(review);

            ctx.SaveChanges();
            return true;
        }

        /// <summary>
        /// Return the number of movies in the database
        /// </summary>
        public int Count
        {
            get => ctx.Movies.Count();
        }

    }
}