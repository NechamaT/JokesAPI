using JokesReact.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactJokes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokesReact.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public JokesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        [Route("getjokes")]
        public List<Joke> GetJokes()
        {
            var repo = new JokesRepository(_connectionString);
            return repo.GetJokes();
        }


        [HttpGet]
        [Route("getrandomjoke")]
        public Joke GetRandomJoke()
        {
            var repo = new JokesRepository(_connectionString);
            return repo.GetJoke();
        }
        [HttpGet]
        [Route("getcounts/{jokeId}")]
        public Counts GetCounts(int jokeId)
        {
            var repo = new JokesRepository(_connectionString);
            return repo.GetCounts(jokeId);
        }
        [HttpPost]
        [Authorize]
        [Route("like")]
        public void Like(LikeViewModel likeViewModel)
        {
            var userRepo = new UserRepository(_connectionString);
            var user = userRepo.GetByEmail(User.Identity.Name);
            var jokesRepo = new JokesRepository(_connectionString);
            jokesRepo.UpdateLikes(user.Id, likeViewModel.JokeId, likeViewModel.Like);

        }
        [HttpGet]
        [Route("getjokebyid")]
        public Joke GetJokeById(int id)
        {
            var repo = new JokesRepository(_connectionString);
            return repo.GetJokeById(id);
        }
        [HttpGet]
        [Route("getuserjokestatus/{jokeid}")]
        public object GetUserJokeStatus(int jokeId)
        {
            UserJokeInteractionStatus status = GetStatus(jokeId);
            return new { status };
        }

        private UserJokeInteractionStatus GetStatus(int jokeId)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return UserJokeInteractionStatus.Unauthenticated;
            }
            var userRepo = new UserRepository(_connectionString);
            var user = userRepo.GetByEmail(User.Identity.Name);
            var jokeRepo = new JokesRepository(_connectionString);
            UserLikedJokes likeStatus = jokeRepo.GetLike(user.Id, jokeId);
            if (likeStatus == null)
            {
                return UserJokeInteractionStatus.NeverInteracted;
            }
            if (likeStatus.Date.AddMinutes(5) < DateTime.Now)
            {
                return UserJokeInteractionStatus.CanNoLongerInteract;
            }
            return likeStatus.Liked
                ? UserJokeInteractionStatus.Liked
                : UserJokeInteractionStatus.Disliked;
        }
    }
}
