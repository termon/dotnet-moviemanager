using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieManager.Web.ViewModels;
using MovieManager.Data.Services;
using MovieManager.Core.Models;
using System.Collections.Generic;

namespace MovieManager.Web.Controllers
{

    [Authorize]
    public class MovieController : BaseController
    {
        private readonly IMovieService _service;
        public MovieController(IMovieService service)
        {
            _service = service;
        }

        // GET: Movies
        public ViewResult Index(string order=null)
        {
            var movies = _service.GetAllMovies(order);
            return View(movies);
        }       

        // GET: Movie/Details/1
        public IActionResult Details(int id)
        {
            var movie = _service.GetMovieById(id);
            if (movie == null)
            {
                Alert("No such movie", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        //// GET: Movie/Create
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Movie/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// AMC - Removed primary key Id from bind list
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Director,Budget,Cast,Year,Duration,Genre,PosterUrl,Plot")] MovieViewModel movie)
        {
            if (ModelState.IsValid)
            {
                movie = MovieViewModel.FromMovie(_service.AddMovie(movie.ToMovie()));
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        //// GET: Movie/Edit/5
        [Authorize(Roles="Admin")]
        public IActionResult Edit(int id)
        { 
            var movie = _service.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(MovieViewModel.FromMovie(movie));
        }

        //// POST: Movie/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Director,Budget,Cast,Year,Duration,Genre,PosterUrl,Plot")] MovieViewModel movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _service.UpdateMovie(movie.ToMovie());
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        //// GET: Movie/Delete/5
        [Authorize(Roles="Admin")]
        public IActionResult Delete(int id)
        {
            var machine = _service.GetMovieById(id);                
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        //// POST: Movie/Delete/5
        [Authorize(Roles="Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteMovie(id); 
            return RedirectToAction(nameof(Index));
        }
      
  
        /// GET: Movie/AddReview/1 
        [HttpGet]
        [Authorize(Roles="Admin,Reviewer")]
    
        public IActionResult AddReview(int id) 
        {
            var movie = _service.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            
            // pre-populate the review - movie relationship and date review created
            var review = new ReviewViewModel { MovieId = id, On = DateTime.Now };
            return View("Review/AddReview",review);
        }

        
        /// POST: Movie/AddReview/1 
        [Authorize(Roles="Admin,Reviewer")]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview([Bind("On,MovieId,Name,Comment,Rating")] ReviewViewModel review)
        {            
            var movie = _service.GetMovieById(review.MovieId);
           
            if (movie == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _service.AddReview(review.ToReview());
                var msg = movie.Title + " - " + review.Comment;
                return RedirectToAction(nameof(Details), new { Id = movie.Id });   
            }
            return View("Review/AddReview", review );                        
        }
        
        
        /// GET: Movie/DeleteReview/1 
         [Authorize(Roles="Admin,Reviewer")]   
        public IActionResult DeleteReview(int id)
        {
            var review = _service.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }
            return View("Review/DeleteReview", review );            
        }
        
        /// POST: Movie/DeleteReview/1 
        [Authorize(Roles="Admin,Reviewer")]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReviewConfirmed(int id)
        {
            var review = _service.GetReviewById(id);
            var ok = _service.DeleteReview(id);
           
            if (!ok)
            {
                return NotFound();
            }
                
            return RedirectToAction(nameof(Details), new { Id = review.MovieId });            
        }

    }
}