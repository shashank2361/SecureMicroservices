﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.ApiServices;
 using Movies.Client.Models;

namespace Movies.Client.Controllers
{

    public class MoviesController : Controller
    {
        private readonly IMovieApiService _movieApiService;

        public MoviesController(IMovieApiService movieApiService)
        {
            _movieApiService = movieApiService ?? throw new ArgumentNullException(nameof(movieApiService));
        }

        // GET: Movies
            [Authorize]
        public async Task<IActionResult> Index()
        {
            await LogTokenAndClaims();
            var movies = await _movieApiService.GetMovies();
            return View(movies);
            //(await _movieApiService.GetMovies()
        }

        public async Task LogTokenAndClaims()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
                Debug.WriteLine($"Identity token: {identityToken}");
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type {claim.Type} - Claim value : {claim.Value}");

            }


        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies= await _movieApiService.GetMovies();
             var movie = movies?.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

             return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Rating,ReleaseDate,ImageUrl,Owner")] Movie movie)
        {
            return View();

            //if (ModelState.IsValid)
            //{
            //    _context.Add(movie);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieApiService.GetMovie(id.ToString());
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,Rating,ReleaseDate,ImageUrl,Owner")] Movie movie)
        {
            return View();

            //if (id != movie.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(movie);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!MovieExists(movie.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var movie = await _context.Movie
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            //return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return View();

            //var movie = await _context.Movie.FindAsync(id);
            //_context.Movie.Remove(movie);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {

            return true;

            // return _context.Movie.Any(e => e.Id == id);
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> OnlyAdmin()
        {
      
            var userInfo = await _movieApiService.GetUserInfo();
            return View(userInfo);



        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        public async Task Logout()
        {
            await HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync( OpenIdConnectDefaults.AuthenticationScheme);
        }

    }
}
