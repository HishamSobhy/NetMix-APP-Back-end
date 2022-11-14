using BL;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace NetMix.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    #region Dependancy Injection
    private readonly IMovie _movies;

    public MoviesController(IMovie movies)
    {
        _movies = movies;
    }
    #endregion

    #region Get
    [HttpGet ("MovieDetails")]
    public async Task<IActionResult> GetMovieDetails(Guid id)
    {
        var result = await _movies.GetMovieDetailsAsync(id);

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest($"No Movie Found With Id {id}");
        }
    } 

    [HttpGet ("MovieByGenre")]
    public async Task<IActionResult> GetMovieByGenre(string genre)
    {
        var result = await _movies.GetByGenreAsync(genre);

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest($"No Movies With Genre {genre}");
        }
    }

    [HttpGet ("GetAllDirectors")]
    public async Task<IActionResult> GetAllDirectors()
    {
        var result = await _movies.GetAllDirectors();

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest($"No Directors Found!!");
        }
    }

    [HttpGet("GetAllActors")]
    public async Task<IActionResult> GetAllActors()
    {
        var result = await _movies.GetAllActors();

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest($"No Actors Found!!");
        }
    }

    [HttpGet("GetAllGenres")]
    public async Task<IActionResult> GetAllGenres()
    {
        var result = await _movies.GetAllGenres();

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest($"No Genres Found!!");
        }
    }

    //Get All Movies from Db
    [HttpGet]
    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _movies.GetAll_Movies_Async();
    }

    // GET Movies by Title
    [HttpGet]
    [Route("Title")]
    public async Task<IActionResult> GetMoviesByTitle(string Title)

    {
        var result = await _movies.GetByMovieNameAsync(Title);


        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest($"No Movies With Name {Title}");
        }
    }

   



    // GET Top10 Movies

    [HttpGet]
    [Route("top6")]
    public async Task<IEnumerable<Movie>> GetToprating()
    {

        var applicant = await _movies.GetAll_Movies_Async();


        var top_rating = (from movie in applicant
                          orderby movie.Rating descending
                          select movie).Take(6);

        return top_rating;
    }

    [HttpGet]
    [Route("top10")]
    public async Task<IEnumerable<Movie>> GetToTop10()
    {

        var applicant = await _movies.GetAll_Movies_Async();


        var top_rating = (from movie in applicant
                          orderby movie.Rating descending
                          select movie).Take(10);

        return top_rating;
    }
    #endregion

}



