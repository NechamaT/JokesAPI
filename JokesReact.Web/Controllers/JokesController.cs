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

    }
}
