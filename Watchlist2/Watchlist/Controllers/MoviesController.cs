﻿using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Watchlist.Services.Contracts;
using Watchlist.Web.Models;

namespace Watchlist.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly IMovieService movieService;
        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var models = await movieService.GetAllMovieAsync();
            return View(models);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddMovieViewModel()
            {
                Genres = await movieService.GetAllGenresAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await movieService.AddMovieAsync(model);
            return RedirectToAction("All", "Movies");
        }
        [HttpGet]
        public async Task<IActionResult> Watched()
        {
            string userId = GetUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("All", "Movies");
            }
            var models = await movieService.GetWatchedMoviesAsync(userId);

            return View(models);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCollection(int movieId)
        {
            string userId = GetUserIdAsync();
            await movieService.AddMovieToCollectionAsync(userId, movieId);

            return RedirectToAction(nameof(All));

        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int movieId)
        {
            string userId = GetUserIdAsync();
            await movieService.DeleteMovieFromCollectionAsync(userId, movieId);

            return RedirectToAction(nameof(Watched));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {

            var model = await movieService.GetMovieByIdAsync(Id);

            model.Genres = await movieService.GetAllGenresAsync();

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int Id, AddMovieViewModel model)
        {

            await movieService.EditMovieAsync(Id, model);

            return RedirectToAction(nameof(All));

        }
    }
}
