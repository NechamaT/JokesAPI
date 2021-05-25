using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReactJokes.Data;

namespace JokesReact.Web
{
    public class JokesRepository
    {
        private readonly string _connectionString;
        public JokesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Joke GetJoke()
        {
            var client = new HttpClient();
            var json = client.GetStringAsync("https://official-joke-api.appspot.com/jokes/programming/random").Result;
            var jokes = JsonConvert.DeserializeObject<List<Joke>>(json);
            var joke = jokes[0];
            using var ctx = new JokesDbContext(_connectionString);
            joke.Id = 0;
            if (!ctx.Jokes.Any(j => j.Punchline == joke.Punchline))
            {
                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
            }
            return ctx.Jokes.Include(j => j.UserLikedJokes).FirstOrDefault(j => j.Punchline == joke.Punchline);
        }

        public List<Joke> GetJokes()
        {
            using var ctx = new JokesDbContext(_connectionString);
            return ctx.Jokes.Include(j => j.UserLikedJokes).ToList();
        }

        public Joke GetJokeById(int id)
        {
            return GetJokes().FirstOrDefault(Joke => Joke.Id == id);
        }

        public void UpdateLikes(int userId, int jokeId, bool like)
        {
            using var ctx = new JokesDbContext(_connectionString);
            var userLikeJoke = ctx.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == jokeId);
            if(userLikeJoke == null)
            {
                ctx.UserLikedJokes.Add(new UserLikedJokes
                {
                    UserId = userId,
                    JokeId = jokeId,
                    Date = DateTime.Now,
                    Liked = like
                });
            }
            else
            {
                userLikeJoke.Liked = like;
                userLikeJoke.Date = DateTime.Now;
            }


        }

        public Counts GetCounts(int jokeId)
        {
            using var ctx = new JokesDbContext(_connectionString);
            return new Counts
            {
                LikedCount = ctx.UserLikedJokes.Count(ulj => ulj.JokeId == jokeId && ulj.Liked == true),
                DislikeCount = ctx.UserLikedJokes.Count(ulj => ulj.JokeId == jokeId && ulj.Liked == false)
            };
        }

        public UserLikedJokes GetLike(int userId, int jokeId)
        {
            using var ctx = new JokesDbContext(_connectionString);
            return ctx.UserLikedJokes.FirstOrDefault(usj => usj.JokeId == jokeId && usj.UserId == userId);
        }

    }

}
