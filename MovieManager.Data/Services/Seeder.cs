using System;
using System.Text;
using System.Collections.Generic;

using MovieManager.Core.Models;

namespace MovieManager.Data.Services
{
    public static class Seeder
    {
        public static void Seed(IUserService user, IMovieService movie)
        {
            IMovieService _movieSvc = movie;
            IUserService _userSvc = user;
            
            _movieSvc.Initialise();  
       
            Movie m = _movieSvc.AddMovie(new Movie
            {
                Title = "Glass",
                Year = 2018,
                Duration = 140,
                Director = "Shyamalan",
                Budget = 23.4,
                Genre = Genre.Action,
                PosterUrl = "https://image.tmdb.org/t/p/w600_and_h900_bestv2/svIDTNUoajS8dLEo7EosxvyAsgJ.jpg",
                Plot = "In a series of escalating encounters, security guard David Dunn uses his supernatural abilities to track Kevin Wendell Crumb, a disturbed man who has twenty-four personalities. Meanwhile, the shadowy presence of Elijah Price emerges as an orchestrator who holds secrets critical to both men.",
                Cast = "James McAvoy, Bruce Willis, Samuel L Jackson"
            });

            _movieSvc.AddReview(new Review
            {
                MovieId = m.Id, Name = "Eileen", On = DateTime.Now, Comment = "Good movie - worth watching", Rating = 5
            });

            _movieSvc.AddReview(new Review
            {
                MovieId = m.Id, Name = "Joe", On = DateTime.Now, Comment = "Really enjoyed this", Rating = 6
            });


            m = _movieSvc.AddMovie(new Movie
            {
                Title = "Shawshank",
                Year = 2007,
                Duration = 180,
                Director = "Darabont",
                Budget = 23.4,
                Genre = Genre.Action,
                PosterUrl = "https://www.themoviedb.org/t/p/w1280/2GgerXCbCMgvt2kLwWEmJWCSG65.jpg",
                Plot = "Framed in the 1940s for the double murder of his wife and her lover, upstanding banker Andy Dufresne begins a new life at the Shawshank prison, where he puts his accounting skills to work for an amoral warden. During his long stretch in prison, Dufresne comes to be admired by the other inmates -- including an older prisoner named Red -- for his integrity and unquenchable sense of hope.",
                Cast = "Morgan Freeman"
            });

            _movieSvc.AddReview(new Review
            {
                MovieId = m.Id, Name = "Fred", On = DateTime.Now, Comment = "Fabulous movie - a must watch", Rating = 8
            });

            _movieSvc.AddReview(new Review
            {
                MovieId = m.Id, Name = "Fred", On = DateTime.Now, Comment = "Best film ever ", Rating = 9
            });

            _movieSvc.AddReview(new Review
            {
                MovieId = m.Id, Name = "Mary", On = DateTime.Now, Comment = "Wow!!!!!", Rating = 9
            });

            m = _movieSvc.AddMovie(new Movie
            {
                Title = "The Green Mile",
                Year = 2007,
                Duration = 150,
                Director = "Daramont",
                Budget = 23.4,
                Genre = Genre.War,
                PosterUrl = "https://image.tmdb.org/t/p/w600_and_h900_bestv2/sOHqdY1RnSn6kcfAHKu28jvTebE.jpg",
                Plot = "A supernatural tale set on death row in a Southern prison, where gentle giant John Coffey possesses the mysterious power to heal people's ailments. When the cell block's head guard, Paul Edgecomb, recognizes Coffey's miraculous gift, he tries desperately to help stave off the condemned man's execution.",
                Cast = "Tom Hanks"
            });

            m = _movieSvc.AddMovie(new Movie
            {
                Title = "Saving Private Ryan",
                Year = 2007,
                Duration = 210,
                Director = "Spielberg",
                Budget = 23.4,
                Genre = Genre.War,
                PosterUrl = "https://www.themoviedb.org/t/p/w1280/1wY4psJ5NVEhCuOYROwLH2XExM2.jpg",
                Plot = "As U.S. troops storm the beaches of Normandy, three brothers lie dead on the battlefield, with a fourth trapped behind enemy lines. Ranger captain John Miller and seven men are tasked with penetrating German-held territory and bringing the boy home.",
                Cast = "Tom Hanks"
            });

            // seed users
            _userSvc.AddUser(
                "Administrator", 
                "admin@mail.com",
                "admin",
                Role.Admin
            );
            _userSvc.AddUser(
                "Guest",
                "guest@mail.com",
                "guest",
                Role.Guest
            );
            _userSvc.AddUser(
                "Reviewer",
                "reviewer@mail.com",
                "reviewer",
                Role.Reviewer
            );

        }

    
    }
}
