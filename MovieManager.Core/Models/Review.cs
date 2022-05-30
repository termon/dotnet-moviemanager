using System;

namespace MovieManager.Core.Models
{
    public class Review
    {     
        public int Id { get; set; }      

        public string Name { get; set; }   

        public string Comment { get; set; }
        public DateTime On { get; set; }

        public int Rating { get; set; }
    
        /// <summary>
        /// EF Dependant Relationship Review -> Movie (1:1)
        /// </summary>
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}