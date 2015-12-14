using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotifyDataClient.Models
{
    public class Album
    {
        public int albumID { get; set; }
        public string name { get; set; }
        public int releaseYear { get; set; }

        public virtual Artist artist { get; set; }
        public virtual ICollection<Song> songs { get; set; }
    }
}