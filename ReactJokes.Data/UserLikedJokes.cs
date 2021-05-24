using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactJokes.Data
{
    public class UserLikedJokes
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }
        public bool Liked { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Joke Joke { get; set; }
    }
}
