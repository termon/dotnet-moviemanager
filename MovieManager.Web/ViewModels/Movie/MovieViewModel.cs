using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieManager.Core.Models;
using MovieManager.Web.Validators;

namespace MovieManager.Web.ViewModels
{

    public class MovieViewModel
    {
        public MovieViewModel()
        {
            Reviews = new List<Review>();
        }
        
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Director { get; set; }     
        public int Year { get; set; }
        
        [Range(1,400)]
        public int Duration { get; set; }   
        
        [Range(1.0,500.0)]
        public double Budget { get; set; }

        [UrlValidator]
        public string PosterUrl { get; set; } 
        
        [EnumDataType(typeof(Genre), ErrorMessage = "Invalid Genre")]
        public Genre Genre { get; set; }

        public string Cast { get; set; }

        [MaxLength(500)]
        public string Plot { get; set; }
        
        public int Rating { get; set; }

        public int ReviewsCount => Reviews.Count;

        public List<Review> Reviews { get; private set; } 

        public Movie ToMovie()
        {
            return new Movie
            {
                Id = this.Id,
                Title = this.Title,
                Director = this.Director,
                Budget = this.Budget,
                Cast = this.Cast,
                Year = this.Year,
                Duration = this.Duration,
                Genre = this.Genre,
                PosterUrl = this.PosterUrl,
                Plot = this.Plot
            };
        }

        public static MovieViewModel FromMovie(Movie m)
        {
            return new MovieViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Director = m.Director,
                Budget = m.Budget,
                Cast = m.Cast,
                Year = m.Year,
                Duration = m.Duration,
                Genre = m.Genre,
                PosterUrl = m.PosterUrl,
                Plot = m.Plot
            };
        }
    }
}