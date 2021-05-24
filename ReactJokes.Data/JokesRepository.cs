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

        public Joke UpdateLikes(UserLikedJokes like) 
        {
            using var ctx = new JokesDbContext(_connectionString);
            var result = ctx.UserLikedJokes.FirstOrDefault(ulj => like.UserId == ulj.UserId && ulj.JokeId == like.JokeId);
            if(result != null)
            {
                ctx.Database.ExecuteSqlInterpolated(@$"UPDATE UserLikedJokes 
                                                    SET Liked ={like.Liked} 
                                                    Time ={like.Date}
                                                    WHERE UserId = {like.UserId}
                                                    AND JokeId = {like.JokeId}");
            }
            else
            {
                ctx.UserLikedJokes.Add(like);
            }
            ctx.SaveChanges();
            return GetJokeById(like.JokeId);
        }


    }

}
