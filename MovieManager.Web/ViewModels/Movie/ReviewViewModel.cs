using System;
using System.ComponentModel.DataAnnotations;
using MovieManager.Core.Models;

namespace MovieManager.Web.ViewModels
{
    public class ReviewViewModel
    {     
        public int Id { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        
        [MaxLength(150)]
        [Required]
        public string Comment { get; set; }
        public DateTime On { get; set; } = DateTime.Now;

        [Range(1,10)]
        [Required]
        public int Rating { get; set; }
    
        public int MovieId { get; set; }

        public Review ToReview()
        {
            return new Review {
                Id = this.Id,
                Name = this.Name,
                On = this.On,
                Comment = this.Comment,
                Rating = this.Rating,
                MovieId = this.MovieId
            };
        }
        public static ReviewViewModel FromReview(Review r)
        {
            return new ReviewViewModel {
                Id = r.Id,
                Name = r.Name,
                On = r.On,
                Comment = r.Comment,
                Rating = r.Rating,
                MovieId = r.MovieId
            };
        }

        
    }
}