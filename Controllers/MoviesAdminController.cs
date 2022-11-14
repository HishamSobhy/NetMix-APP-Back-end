using BL;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NetMix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "DashBoard")]
    public class MoviesAdminController : ControllerBase
    {
        #region Dependancy Injection
        private readonly IMovie _movies;
        private readonly IActor _actor;
        private readonly IDirector _director;
        private readonly IGenre _genre;
        private readonly NetMixDbContext _context;

        public MoviesAdminController(IMovie movies, IActor actor, IDirector director, IGenre genre, NetMixDbContext context)
        {
            _movies = movies;
            _actor = actor;
            _director = director;
            _genre = genre;
            _context = context;
        }
        #endregion

        #region Add
        [HttpPost("AddMovie")]
        public async Task<ActionResult<MovieReadDTO>> AddMovie(MovieAddDTO movieAddDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _movies.AddMovieAsync(movieAddDTO);
           
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Movie Cannot be added");
            }
        }

        [HttpPost("AddActor")]
        public async Task<ActionResult<ActorReadDTO?>> AddActor(string name)
        {
            var result = await _actor.AddActor(name);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Actor Cannot be added");
            }
        }

        [HttpPost("AddDirector")]
        public async Task<ActionResult<Director?>> AddDirector(string name)
        {
            var result = await _director.AddDirector(name);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Director Cannot be added");
            }
        }

        [HttpPost("AddGenre")]
        public async Task<ActionResult<GenreReadDTO?>> AddGenre(string name)
        {
            var result = await _genre.AddGenre(name);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Genre Cannot be added");
            }
        }
        #endregion

        #region Update
        [HttpPut("Movie")]
        public async Task<ActionResult<MovieReadDTO>> UpdateMovie(MovieUpdateDTO movie, Guid id)
        {
            var result = await _movies.UpdateAsync(movie, id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Movie Cannot Be Updated");
            }
        }
        #endregion

        #region Delete
        [HttpDelete("Movie")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            _movies.DeleteAsync(id);
            return NoContent();
        }
        [HttpDelete("Actor")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            _actor.DeleteActor(id);
            return NoContent();
        }

        [HttpDelete("Director")]
        public async Task<IActionResult> DeleteDirector(Guid id)
        {
            _director.DeleteDirector(id);
            return NoContent();
        }
        [HttpDelete("Genre")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            _genre.DeleteGenre(id);
            return NoContent();
        }
        #endregion
    }
}
