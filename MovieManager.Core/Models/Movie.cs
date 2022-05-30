using System.Collections.Generic;
using System.Linq;


namespace MovieManager.Core.Models
{
    public enum Genre
    {
        Action, Comedy, Family, Horror, Romance, SciFi, Thriller, Western, War
    }
    
    public class Movie
    {
        public Movie()
        {
            Reviews = new List<Review>();
        }
        
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Director { get; set; }     
        public int Year { get; set; }

        public int Duration { get; set; }   
        
        public double Budget { get; set; }

        public string PosterUrl { get; set; } 
        
        public Genre Genre { get; set; }

        public string Cast { get; set; }

        public string Plot { get; set; }
        
        /// <summary>
        /// ReadOnly Property - Calculates Rating % based on average of all reviews
        /// </summary>
        public int Rating
        {
            get
            {
                var count = Reviews.Count > 0 ? Reviews.Count : 1;
                return Reviews.AsEnumerable().Sum(r => r.Rating) / count * 10;
            }
        }

        /// <summary>
        /// ReadOnly Property - Calculates Number of reviews based on review list
        /// </summary>
        public int ReviewsCount =>  Reviews.Count; //Reviews?.Count ?? 0;


        /// <summary>
        /// EF Relationship - movie -> review (1:N)
        /// </summary>
        public List<Review> Reviews { get; set; } 

    }
}